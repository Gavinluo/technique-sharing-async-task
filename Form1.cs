using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TechniqueSharingAsyncTask
{
    public partial class Form1 : Form
    {
        // 日志写入需要 UI 线程且避免交错，所以单独的锁与 Invoke 协作。
        private readonly object _logSync = new object();

        // 并发计数器演示用的锁。
        private readonly object _counterLock = new object();

        // async 任务的入口互斥，确保“防重复启动”。
        private readonly SemaphoreSlim _taskGate = new SemaphoreSlim(1, 1);

        // 正在运行的 async 任务的取消源。
        private CancellationTokenSource _cts;

        // 演示运行标识递增，用于日志区分 Run#。
        private int _runIdCounter;

        // DoEvents 重入演示的共享状态（故意共享以暴露错乱）。
        private int _sharedDoEventsStep;

        // 勾选“Guard DoEvents reentrancy”后的互斥标志。
        private bool _doEventsRunning;

        // 并发计数器的共享变量。
        private int _sharedCounter;

        private VersionedDocument _document = new VersionedDocument
        {
            Data = "Initial content",
            Version = 1
        };

        // 模拟两个编辑窗口各自持有的版本号。
        private int _editorAVersion;
        private int _editorBVersion;

        public Form1()
        {
            InitializeComponent();
            statusProgress.Minimum = 0;
            statusProgress.Maximum = 100;
            statusProgress.Value = 0;
            statusLabel.Text = "Ready.";
            btnCancel.Enabled = false;
            _editorAVersion = _document.Version;
            _editorBVersion = _document.Version;
            txtEditorA.Text = _document.Data;
            txtEditorB.Text = _document.Data;
            RefreshVersionLabel();
        }

        private void btnSyncWork_Click(object sender, EventArgs e)
        {
            // 纯同步长任务：故意卡住 UI 线程，演示“假死”现象。
            int runId = ++_runIdCounter;
            Log($"[Run#{runId}] Sync work started.");
            RunCpuHeavyLoop(runId, useDoEvents: false, total: 300);
            Log($"[Run#{runId}] Sync work completed.");
        }

        private void btnDoEventsWork_Click(object sender, EventArgs e)
        {
            // 可选防重入开关：演示“最小止血方案”如何阻止重入。
            if (chkGuardReentrancy.Checked && _doEventsRunning)
            {
                Log("[Guard] DoEvents work already running, guard blocked reentrancy.");
                return;
            }

            int runId = ++_runIdCounter;
            bool guardEnabled = chkGuardReentrancy.Checked;
            _doEventsRunning = guardEnabled || _doEventsRunning;

            Log($"[Run#{runId}] DoEvents work started.");
            try
            {
                int total = 300;
                for (int step = 1; step <= total; step++)
                {
                    // 故意写入共享字段，让多次点击时产生交错错乱。
                    _sharedDoEventsStep = step;
                    DoCpuStep(step);
                    UpdateStatus(runId, step, total);
                    Log($"[Run#{runId}] Step {step} | SharedStep={_sharedDoEventsStep}");
                    // DoEvents 把控制权交给消息泵，新的 Click 会在此时进来。
                    Application.DoEvents();
                }

                Log($"[Run#{runId}] DoEvents work finished.");
            }
            finally
            {
                if (guardEnabled)
                {
                    _doEventsRunning = false;
                }
                UpdateStatus(0, 0, 1);
            }
        }

        private async void btnTaskWork_Click(object sender, EventArgs e)
        {
            // SemaphoreSlim 确保防重复启动；WaitAsync(0) 实现非阻塞尝试。
            if (!await _taskGate.WaitAsync(0))
            {
                Log("[Task] Another task run is already in progress.");
                return;
            }

            int runId = ++_runIdCounter;
            btnTaskWork.Enabled = false;
            btnCancel.Enabled = true;
            _cts = new CancellationTokenSource();
            CancellationToken token = _cts.Token;
            Log($"[Run#{runId}] Task/async work started.");

            var progress = new Progress<int>(step => UpdateStatus(runId, step, 300));

            try
            {
                // Task.Run 把 CPU 密集循环移出 UI 线程，Progress 将进度切回 UI。
                await Task.Run(() =>
                {
                    for (int step = 1; step <= 300; step++)
                    {
                        token.ThrowIfCancellationRequested();
                        DoCpuStep(step);
                        ((IProgress<int>)progress).Report(step);
                    }
                }, token);

                Log($"[Run#{runId}] Task/async work completed.");
            }
            catch (OperationCanceledException)
            {
                Log($"[Run#{runId}] Task/async work cancelled.");
            }
            finally
            {
                // 恢复 UI 状态与互斥。
                UpdateStatus(0, 0, 1);
                btnTaskWork.Enabled = true;
                btnCancel.Enabled = false;
                _cts = null;
                _taskGate.Release();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (_cts != null)
            {
                _cts.Cancel();
                Log("[Cancel] Cancellation requested.");
            }
        }

        private void btnClickMe_Click(object sender, EventArgs e)
        {
            // 用于证明 UI 是否活着：同步场景会卡住，看不到这条日志。
            Log("[UI] Click Me pressed - UI thread is responsive.");
        }

        private async void btnLockCounter_Click(object sender, EventArgs e)
        {
            int taskCount = 8;
            int perTask = 20000;
            _sharedCounter = 0;

            Log($"[Lock] Starting {taskCount} tasks, {perTask} increments each. UseLock={chkUseLock.Checked}");
            List<Task> tasks = Enumerable.Range(0, taskCount)
                .Select(_ => Task.Run(() => IncrementCounter(perTask, chkUseLock.Checked)))
                .ToList();

            await Task.WhenAll(tasks);
            Log($"[Lock] Expected count={taskCount * perTask}, Actual={_sharedCounter}");
        }

        private void btnVersionSaveA_Click(object sender, EventArgs e)
        {
            // 模拟“窗口 A”保存；版本校验失败则提示冲突。
            SaveFromEditor("A", txtEditorA.Text, ref _editorAVersion);
        }

        private void btnVersionSaveB_Click(object sender, EventArgs e)
        {
            // 模拟“窗口 B”保存。
            SaveFromEditor("B", txtEditorB.Text, ref _editorBVersion);
        }

        private void RunCpuHeavyLoop(int runId, bool useDoEvents, int total)
        {
            for (int step = 1; step <= total; step++)
            {
                DoCpuStep(step);
                UpdateStatus(runId, step, total);
                Log($"[Run#{runId}] Step {step}/{total}");
                if (useDoEvents)
                {
                    Application.DoEvents();
                }
            }

            UpdateStatus(0, 0, 1);
        }

        private void DoCpuStep(int step)
        {
            // 人为制造 CPU 消耗 + 轻微 Sleep，保证循环耗时可感知。
            double x = 0;
            for (int i = 0; i < 2000; i++)
            {
                x += Math.Sqrt(step + i % 5);
            }

            Thread.Sleep(5);
        }

        private void UpdateStatus(int runId, int step, int total)
        {
            int percent = Math.Min(100, Math.Max(0, (int)(step * 100.0 / total)));
            void UpdateUi()
            {
                statusLabel.Text = runId == 0
                    ? "Ready."
                    : $"Run#{runId}: Step {step}/{total} ({percent}%)";
                statusProgress.Value = Math.Min(statusProgress.Maximum, Math.Max(statusProgress.Minimum, percent));
            }

            if (statusStrip1.InvokeRequired)
            {
                statusStrip1.BeginInvoke(new Action(UpdateUi));
            }
            else
            {
                UpdateUi();
            }
        }

        private void Log(string message)
        {
            // 确保日志追加在 UI 线程执行，并保持滚动到最后一行。
            string line = $"{DateTime.Now:HH:mm:ss.fff} - {message}";
            void AddLog()
            {
                lock (_logSync)
                {
                    lstLog.Items.Add(line);
                    lstLog.TopIndex = lstLog.Items.Count - 1;
                }
            }

            if (lstLog.InvokeRequired)
            {
                lstLog.BeginInvoke(new Action(AddLog));
            }
            else
            {
                AddLog();
            }
        }

        private void IncrementCounter(int times, bool useLock)
        {
            for (int i = 0; i < times; i++)
            {
                if (useLock)
                {
                    // 使用 lock/Interlocked 保护共享变量，结果应稳定等于预期。
                    lock (_counterLock)
                    {
                        _sharedCounter++;
                    }
                }
                else
                {
                    // 无锁模式：演示最终计数往往小于预期。
                    _sharedCounter++;
                }
            }
        }

        private void SaveFromEditor(string editorName, string text, ref int editorVersion)
        {
            if (editorVersion != _document.Version)
            {
                Log($"[Version] Editor {editorName} conflict. LocalVersion={editorVersion}, Current={_document.Version}. Save rejected.");
                return;
            }

            // 乐观锁思路：版本一致才写入，并将版本号 +1。
            _document.Data = text;
            _document.Version++;
            editorVersion = _document.Version;
            Log($"[Version] Editor {editorName} saved successfully. New version={_document.Version}.");
            RefreshVersionLabel();
        }

        private void RefreshVersionLabel()
        {
            lblVersion.Text = $"Version: {_document.Version}";
        }
    }

    internal class VersionedDocument
    {
        public string Data { get; set; }

        public int Version { get; set; }
    }
}
