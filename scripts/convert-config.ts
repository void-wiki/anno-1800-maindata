import pth from 'path';
import fs from 'fs-extra';
import globby from 'globby';
import execa from '@void-aurora/toolkit/node_modules/execa';
import { dataDir, convertedDir, pathConfigconv, findFiles } from './scripts-utils';

const configDir = pth.resolve(convertedDir, 'config');

async function jsonEscapeBack(path: string): Promise<void> {
  const json = await fs.readJSON(path);
  // eslint-disable-next-line @typescript-eslint/naming-convention
  await fs.writeJSON(path, json, { spaces: 0, EOL: '\n' });
}

async function convert(): Promise<void> {
  await fs.remove(configDir);

  console.time('convert costs');

  const { command, stdout } = await execa('dotnet', [pathConfigconv, dataDir, convertedDir]);
  console.log(command);
  console.log(stdout);

  const files = await findFiles(['**/*.json'], { cwd: configDir });

  // un-escapes all of json files
  await Promise.all(files.map(async f => jsonEscapeBack(pth.resolve(configDir, f))));

  console.timeEnd('convert costs');
}

convert().catch(console.error);
