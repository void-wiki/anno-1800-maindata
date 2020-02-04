# Anno 1800 Main Data

This repo stores main data extracted from Anno 1800.

## Files

### RDA Files

Tool: [RDAExplorer](https://github.com/lysannschlegel/RDAExplorer)

There are 14 RDA files need to be extracted: `<path-to>/Anno 1800/maindata/data0.rda` ~ `<path-to>/Anno 1800/maindata/data13.rda`

### Checksum

#### `checksum.db`

Copy from `<path-to>/Anno 1800/maindata/checksum.db`.

#### `sha512sum`

Go into the maindata directory and run the command in bash:

```sh
find data*.rda -type f -exec sha512sum "{}" + > sha512sum
```

## Others

### Play .bk2 video file

Use RADTools: http://www.radgametools.com/cn/bnkdown.htm

https://www.anno-union.com/en/union-update-into-the-new-year-with-gu6-2/
