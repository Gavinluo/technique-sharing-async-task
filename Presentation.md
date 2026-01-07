# 并发 / 锁 / 版本戳 / 异步 

## 概要
- 目标：让界面不卡（UI responsiveness）且数据不乱（correctness）。
- 核心问题：为什么 UI 会卡？`Application.DoEvents()` 的风险是什么？如何用 `Task`/`async` 与并发控制来解决？

## 关键定义
- 并发（Concurrency）：在同一时间段内，有多个执行流并发推进。
- 异步（Async）：不阻塞等待，尤其是不要阻塞 UI 线程。
- 重要：`异步 ≠ 并发`，但引入异步常会带来并发场景（例如重复启动任务）。

## UI 卡顿的根因
- UI 线程同时负责事件处理和消息泵（重绘、输入、关闭等）。
- 在 UI 线程执行同步长任务会占满消息泵，导致窗口无响应。
- 原则：不要在 UI 线程做长时间的同步工作。

## `Application.DoEvents()`（权宜方案）
- 优点：临时把控制权交还消息泵，使界面“看起来”可响应。
- 风险：允许新的消息在任务未完成时插队，产生重入（reentrancy）。
- 后果：共享状态被交错读写、进度回退、出现难以复现的并发 bug。

## 重入示例（要点说明）
- 场景：循环内调用 `Application.DoEvents()` 并更新进度。
- 操作：任务运行中再次点击“开始”按钮。
- 观察：日志输出交错、进度条跳变、集合或文件资源出错，甚至死锁。

## 最小止血方案（可立即落地）
- 启动时禁用开始按钮，任务结束时恢复。
- 使用运行标志位或互斥（例如 `bool isRunning` 或 `SemaphoreSlim`），重复启动直接 return。
- 节流 UI 更新（例如每 50ms 或每 N 次循环才更新一次进度）。
- 说明：这些是临时措施，长期方案是迁移到 `async`/`Task`。

## 推荐实践：`Task` / `async` 正确结构
- UI 线程：只负责事件响应与展示。
- 后台工作：使用 `Task.Run`（CPU 密集）或真正的异步 IO（IO 密集）。
- 进度回报：使用 `IProgress<T>` 或通过同步回调切回 UI 线程。
- 支持取消：使用 `CancellationToken`，在任务中定期检查并响应取消。

## 并发控制：启动防重入与取消
- 防重复启动：按钮禁用 + `SemaphoreSlim` / 标志位。
- 取消：`CancellationTokenSource.Cancel()`，在后台任务中响应并清理资源。
- UI 展示：提供运行/取消/完成的可视反馈与可靠的进度条。

## 锁（悲观并发）要点
- 适用场景：同进程内共享内存的并发访问。
- 常用手段：`lock`、`Interlocked`、并发容器（如 `ConcurrentDictionary`）。
- 原则：缩小锁的范围，不在锁内做 IO 或长时间等待。

## 版本戳（乐观并发）要点
- 适用场景：跨请求、跨窗口或跨用户编辑导致的冲突（如并发保存）。
- 做法：对象携带 `Version`（或时间戳），保存时比较版本一致性再写入并 +1。
- 处理策略：版本不一致时提示冲突、拒绝保存或按策略重试/合并。

## 决策速查表
- UI 卡顿：优先采用 `async`/`await`，避免长期依赖 `DoEvents`。
- CPU 密集工作：`Task.Run` + 进度回报 + 取消支持。
- 防重复启动：按钮禁用 + 互斥（`SemaphoreSlim` / 标志位）。
- 内存共享：使用 `lock` / `Interlocked` / 并发容器。
- 跨请求冲突：采用版本戳（乐观并发）。

## Demo 指南（快速操作）
- `Sync Work (UI Freeze)`：展示 UI 被阻塞的现象。
- `DoEvents Work`：展示看似响应但会发生重入的问题。
- `Task Work`：展示 `Task.Run` + `IProgress` + 取消 + 防重复启动 的正确实现。
- `Guard DoEvents reentrancy`：勾选后展示止血方案的效果。

## 简短结语
- 两个工程目标：不卡（UI responsiveness）+ 不乱（correctness）。
- `Application.DoEvents()` 可用于应急，但代价很高，长期应迁移到可控的 `async`/`Task` + 并发控制。

---

## 附录：现场可直接贴的关键代码片段

禁止重复启动 / 互斥（示例）：
```
if (isRunning) return;
try { isRunning = true; /* start work */ }
finally { isRunning = false; }
```

使用 `IProgress<T>` 回报进度（示例）：
```
var progress = new Progress<int>(p => progressBar.Value = p);
await Task.Run(() => DoWork(progress, token));
```

响应取消（示例）：
```
if (token.IsCancellationRequested) return;
