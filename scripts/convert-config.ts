import pth from 'path';
import fs from 'fs-extra';
import globby from 'globby';
import execa from '@void-aurora/toolkit/node_modules/execa';
import { dataDir, convertedDir, pathConfigconv, findFiles } from './scripts-utils';

async function jsonEscapeBack(path: string): Promise<void> {
  const json = await fs.readJSON(path);
  // eslint-disable-next-line @typescript-eslint/naming-convention
  await fs.writeJSON(path, json, { spaces: 2, EOL: '\n' });
}

async function convert(): Promise<void> {
  await fs.remove(pth.resolve(convertedDir, 'config'));

  console.time('convert costs');

  const { command, stdout } = await execa('dotnet', [pathConfigconv, dataDir, convertedDir]);
  console.log(command);
  console.log(stdout);

  const guiDirDest = pth.resolve(convertedDir, 'config', 'gui');
  const files = await findFiles(['*.json'], { cwd: guiDirDest });
  await Promise.all(files.map(async f => jsonEscapeBack(pth.resolve(guiDirDest, f))));

  console.timeEnd('convert costs');
}

convert().catch(console.error);
