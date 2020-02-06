# Anno 1800 Main Data

This repo stores main data extracted from Anno 1800.

## Usage

Load files from jsDelivr (an open source CDN).

Example:

<img src="https://cdn.jsdelivr.net/gh/void-wiki/anno-1800-maindata/data-converted/ui/2kimages/main/3dicons/resident/icon_resident_farmer.png" />

```htm
<!-- replace '<version>', check out releases -->
<img
  src="https://cdn.jsdelivr.net/gh/void-wiki/anno-1800-maindata@<version>/data-converted/ui/2kimages/main/3dicons/resident/icon_resident_farmer.png"
/>
```

> ⚠ **NOTE**: Must use **VERSION** in the url for production.

## Getting Start

### Environment

- nodejs >=13.0.0
- yarn >=1.21.0 & <2.0.0
- dotnet core sdk 3.1.0

### Install development dependencies

```sh
yarn install
```

### Preparation

```sh
yarn prepare
```

### Checksum

#### `checksum.db`

Copy from `<path-to>/Anno 1800/maindata/checksum.db`.

#### `sha512sum`

Go into the maindata directory and run the command in bash:

```sh
find data*.rda -type f -exec sha512sum "{}" + > sha512sum
```

### Extract RDA Files

Tool: [RDAExplorer](https://github.com/lysannschlegel/RDAExplorer)

All of `data<X>.rda` RDA files need to be extracted: `<path-to>/Anno 1800/maindata/data0.rda` ~ `<path-to>/Anno 1800/maindata/data13.rda`.

The directory structure should like this:

```
anno-1800-maindata
├─checksum
│  ├─checksum.db
│  └─sha512sum
├─data
│  ├─blacklists
│  ├─config
│  ├─dlc01
│  ├─dlc02
│  ├─dlc03
│  ├─dm
│  ├─fonts
│  ├─graphics
│  ├─infotips
│  ├─script
│  ├─sessions
│  ├─shaders
│  ├─sound
│  └─ui
├─data-converted
├─...
├─README.md
└─...
```

### Convert Images from DDS to PNG

```sh
yarn conv:dds2png
```

## Others

### Tools

- [RDAExplorer](https://github.com/lysannschlegel/RDAExplorer): extract .rda files
- [DirectXTex](https://github.com/microsoft/DirectXTex): convert .dds images to .png with `texconv.exe`.
- [RADTools](http://www.radgametools.com/bnkdown.htm): play .bk2 video file
