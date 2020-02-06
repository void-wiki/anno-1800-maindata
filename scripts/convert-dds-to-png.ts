import pth from 'path';
import fs from 'fs-extra';
import execa from '@void-aurora/toolkit/node_modules/execa';
import {
  rootDir,
  tempDir,
  dataDir,
  convertedDir,
  pathTexconv,
  pathImgconv,
  findFiles,
  TaskFunction,
  parallelQueue,
} from './scripts-utils';

const includes = ['ui/2kimages'];

async function convert(): Promise<void> {
  console.time('find');
  const dirs = await findFiles(
    includes.map(p => `${p}/**/*`),
    {
      cwd: dataDir,
      onlyDirectories: true,
    },
  );
  const ddsFiles = await findFiles(
    includes.map(p => `${p}/**/*.dds`),
    {
      cwd: dataDir,
    },
  );
  console.timeEnd('find');

  // create directories
  await Promise.all(dirs.map(async dir => fs.mkdirp(pth.resolve(tempDir, dir))));
  await Promise.all(dirs.map(async dir => fs.mkdirp(pth.resolve(convertedDir, dir))));

  const tasks: TaskFunction[] = ddsFiles.map<TaskFunction>(dds => async (): Promise<void> => {
    // Convert .dds files to .png files
    // NOTE: these .png files have weird white color
    const ddsSrc = pth.resolve(dataDir, dds);
    const pngTemp = pth.resolve(tempDir, dds.replace(/\.dds$/u, '.PNG'));
    await execa(pathTexconv, [ddsSrc, '-o', pth.dirname(pngTemp), '-ft', 'png', '-y']);

    // Convert weird .png files to correct color
    const png = pth.resolve(
      convertedDir,
      dds.replace(/\.dds$/u, '.png').replace(/_0\.png/u, '.png'),
    );
    await execa('dotnet', [pathImgconv, pngTemp, png]);
  });

  console.time('convert');
  await parallelQueue(tasks);
  console.timeEnd('convert');
}

convert().catch(console.error);
