import pth from 'path';
import fs from 'fs-extra';
import { dataDir, findFiles } from './scripts-utils';

async function find(): Promise<void> {
  // *.dds
  const ddsFiles = await findFiles(['**/*.dds'], { cwd: dataDir });
  // *_0.dds
  const dds0Files = [] as string[];
  // *.dds with *.png
  const ddsPngFiles = [] as string[];
  // *_0.dds width *.png
  const dds0PngFiles = [] as string[];
  // *_0..ds with *_0.png
  const dds0Png0Files = [] as string[];

  await Promise.all(
    ddsFiles.map(async dds => {
      if (dds.endsWith('_0.dds')) {
        dds0Files.push(dds);
        const png = pth.resolve(dataDir, dds.replace(/_0\.dds$/u, '.png'));
        const png0 = pth.resolve(dataDir, dds.replace(/_0\.dds$/u, '_0.png'));
        if (await fs.pathExists(png)) {
          dds0PngFiles.push(png);
        }
        if (await fs.pathExists(png0)) {
          dds0Png0Files.push(png0);
        }
      } else {
        const png = pth.resolve(dataDir, dds.replace(/\.dds$/u, '.png'));
        if (await fs.pathExists(png)) {
          ddsPngFiles.push(png);
        }
      }
    }),
  );

  console.log('dds', ddsFiles.length);
  console.log('dds0', dds0Files.length);
  console.log('ddsPng', ddsPngFiles.length);
  console.log('dds0Png', dds0PngFiles.length);
  console.log('dds0Png0', dds0Png0Files.length);
}

find().catch(console.error);
