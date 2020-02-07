import pth from 'path';
import globby from 'globby';

/**
 * The path to project root directory.
 */
export const rootDir = pth.resolve(__dirname, '..');

/**
 * The path to directory '.tmp'.
 */
export const tempDir = pth.resolve(rootDir, '.tmp');

/**
 * The path to directory `data`.
 */
export const dataDir = pth.resolve(rootDir, 'data');

/**
 * The path to directory `data-converted`
 */
export const convertedDir = pth.resolve(rootDir, 'data-converted');

/**
 * The path to `configconv.dll`.
 */
export const pathConfigconv = pth.resolve(
  rootDir,
  'packages',
  'configconv',
  'bin',
  'Debug',
  'netcoreapp3.1',
  'configconv.dll',
);

/**
 * The path to directory `tools`.
 */
export const toolsDir = pth.resolve(rootDir, 'tools');

/**
 * The path to `texconv.exe`.
 */
export const pathTexconv = pth.resolve(toolsDir, 'texconv.exe');

/**
 * The path to `imgconv.dll`.
 */
export const pathImgconv = pth.resolve(
  rootDir,
  'packages',
  'imgconv',
  'bin',
  'Debug',
  'netcoreapp3.1',
  'imgconv.dll',
);

/**
 * Find files with globby.
 * @param patterns the glob patterns
 * @param options the globby options
 */
export async function findFiles(
  patterns: string[],
  options?: globby.GlobbyOptions,
): Promise<string[]> {
  const time1 = await globby(patterns, options);
  const time2 = await globby(patterns, options);
  const time3 = await globby(patterns, options);

  return [...new Set([...time1, ...time2, ...time3])].sort((a, b) => a.localeCompare(b));
}

// eslint-disable-next-line @typescript-eslint/no-explicit-any
export type TaskFunction = () => Promise<any>;

/**
 * Run tasks parallel
 * @param tasks the array of task functions
 * @param workers the number of workers for running tasks
 */
export async function parallelQueue(
  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  tasks: TaskFunction[],
  workers: number = 16,
): Promise<void> {
  const taskPool = [...tasks];
  const invoke = async (): Promise<void> => {
    if (taskPool.length === 0) {
      return;
    }
    const currentTask = taskPool.shift() as TaskFunction;
    await currentTask();
    await invoke();
  };

  const limit = workers < tasks.length ? workers : tasks.length;
  const runningTasks = [] as Promise<void>[];
  for (let i = 0; i < limit; i++) {
    runningTasks.push(invoke());
  }
  await Promise.all(runningTasks);
}
