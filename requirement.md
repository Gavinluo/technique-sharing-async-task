# Codex 提示词：生成 .NET Framework 4.6 WinForms Demo（含 DoEvents 反例与 async 正解）

你是 Codex。请为我生成一个可运行的 **C# WinForms** Demo 项目，要求如下：

---

## 目标

这个项目用于培训演示以下主题（必须都能通过 UI 按钮直观看到）：

1) **同步长任务导致 UI 无响应**（反例）  
2) **DoEvents + 状态栏进度** 让 UI “看起来不卡”（现状做法）  
3) **DoEvents 引发重入（Reentrancy）**：任务未结束时重复点击导致重复进入、状态错乱（反例重点）  
4) **async/Task.Run + 进度汇报 + 可取消 + 防重复启动**（推荐做法）  
5) （可选加分）展示一个简单的 **lock vs 版本戳**：  
   - lock：保护共享计数器  
   - 版本戳：模拟“保存时版本不一致则提示冲突”

---

## 技术约束

- **.NET Framework 4.6**
- **WinForms**
- 单项目即可（一个 Form1 也行），不要引入外部 NuGet 包
- 代码可读性优先：注释清晰、日志输出清楚、现象可复现

---

## UI 设计（必须实现）

Form1 上放置以下控件（布局合理即可）：

1) 顶部一排按钮（或两排也可）：
- `btnSyncWork` 文案：`Sync Work (UI Freeze)`
- `btnDoEventsWork` 文案：`DoEvents Work (Looks Responsive)`
- `btnTaskWork` 文案：`Task/Async Work (Recommended)`
- `btnCancel` 文案：`Cancel`
- `btnClickMe` 文案：`Click Me (UI still alive?)`
- （可选）`btnLockCounter`：`Counter with/without Lock`
- （可选）`btnVersionSave`：`Versioned Save Conflict`

2) 中间一个 `ListBox`（或 `TextBox Multiline`）用来输出日志：`lstLog`
- 日志必须包含时间与 RunId（例如 Run#1, Run#2）以便看出交错

3) 底部 `StatusStrip`
- `ToolStripStatusLabel`：显示当前状态/步骤
- `ToolStripProgressBar`：显示进度（0-100）

---

## 行为与演示要求（必须严格实现）

### A. Sync Work：制造 UI 无响应
- `btnSyncWork_Click` 在 UI 线程执行一个耗时循环（例如 300 步）
- 每步做一点 CPU 工作（不能只 Thread.Sleep，否则现象不稳定；可以混合 Sleep + 小计算）
- 更新进度条与状态栏（但 UI 很可能不刷新）
- 运行期间 UI 应明显卡顿，`btnClickMe` 无法立即响应，关闭也卡

### B. DoEvents Work：进度 + DoEvents（现状做法）
- `btnDoEventsWork_Click` 在 UI 线程执行耗时循环（例如 300 步）
- 每步更新状态栏与进度条
- 每步（或每隔几步）调用 `Application.DoEvents()`
- 现象：UI 仍可响应，进度条能刷新

### C. DoEvents 重入反例（重点必须可复现）
必须让观众能“用手点出来”重入问题：

- DoEvents Work 运行过程中，用户再次点击 `btnDoEventsWork`
- 由于 DoEvents 处理了消息泵，新的 Click 会进入同一个 handler
- 要求现象明显：日志中出现两个 RunId 交错输出，进度跳变、状态错乱等

实现方式建议（你可自行选择，但必须稳定复现）：
- 在 `btnDoEventsWork_Click` 中不禁用按钮，不做互斥
- 每次点击分配一个 `runId`（递增 int）并写入日志
- 循环中写共享状态（例如共享 List、共享字段 currentStep、或共享文件写入）
- 在日志里输出：`[Run#X] Step i ...`
- 让两个 Run 的日志必然交错（例如每步 DoEvents + 少量 Sleep/计算）

同时提供一个可选开关（例如 `CheckBox chkGuardReentrancy`）：
- 关闭时：展示重入灾难（默认）
- 勾选时：启用“止血方案”（互斥/禁用按钮/标志位），对比效果

### D. Task/Async 正解（必须包含：进度、取消、防重复）
`btnTaskWork_Click` 必须是 `async void`，实现：

- 使用 `CancellationTokenSource` 配合 `btnCancel`
- 使用 `IProgress<int>` 或 `Progress<int>` 在 UI 线程更新进度条与状态栏
- 使用 `Task.Run` 执行 CPU 循环（例如 300 步）
- 防止重复启动：用以下任一方式（必须有效）：
  - 禁用 `btnTaskWork` 直到任务结束
  - 或使用 `SemaphoreSlim(1,1)` 来保护任务入口（建议）
- 运行期间 `btnClickMe` 必须能立即响应
- 取消后要有明确日志：`Cancelled`，并恢复按钮状态

### E. Click Me：证明 UI 是否活着
- `btnClickMe_Click` 立即在日志中输出一条：`UI Clicked at ...`
- 用于证明 Sync 卡死 vs DoEvents/Task 可响应

---

## 代码结构要求

- 清晰拆分方法：
  - `DoCpuStep(int step)`：模拟 CPU 消耗
  - `Log(string msg)`：统一日志输出（带时间）
  - `UpdateStatus(runId, step, total)`：更新状态栏与进度条
- 日志必须线程安全：
  - 如果从后台线程写日志，必须 `Invoke/BeginInvoke` 切回 UI
  - 或日志只在 UI 线程写，后台通过 Progress 报告

---

## （可选加分）锁 vs 版本戳模块

### 1) lock 演示（简单计数器）
- `btnLockCounter` 点击后启动多个任务并发 `counter++`
- 对比两次运行：
  - 不加锁/不使用 Interlocked：结果小于预期
  - 使用 lock 或 Interlocked：结果等于预期
- 日志输出最终 counter 值

### 2) 版本戳冲突（模拟保存）
- 使用一个对象 `{ Data, Version }`
- 模拟两个“编辑窗口”（可用两个按钮或两个文本框代表 A/B 编辑）
- 保存时校验 Version：
  - Version 一致才保存并 Version++
  - 不一致则提示冲突（MessageBox 或日志提示）

---

## 交付内容

请输出完整可编译运行的项目内容（至少包含）：
- `Program.cs`
- `Form1.cs`
- `Form1.Designer.cs`（或用代码动态创建控件也可以，但要完整）
- 说明：如何运行、如何触发 DoEvents 重入反例、如何对比 Task/Async 正解

请确保在 **Visual Studio 2019/2022** 下创建 .NET Framework 4.6 WinForms 项目即可直接运行。

---

## 额外提示（重要）

- DoEvents 重入现象必须稳定可复现：请在循环中加入少量 `Thread.Sleep(5~10ms)` 或可控 CPU 消耗，确保用户有时间再次点击按钮并触发重入。
- 进度条范围固定 0-100，step->percent 映射清晰。
- 取消要可靠：后台循环中定期检查 `token.ThrowIfCancellationRequested()`。
