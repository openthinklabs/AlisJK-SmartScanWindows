/* Этот файл является частью библиотеки Saraff.Tiff.NET
 * © SARAFF SOFTWARE (Кирножицкий Андрей), 2014.
 * Saraff.Tiff.NET - свободная программа: вы можете перераспространять ее и/или
 * изменять ее на условиях Меньшей Стандартной общественной лицензии GNU в том виде,
 * в каком она была опубликована Фондом свободного программного обеспечения;
 * либо версии 3 лицензии, либо (по вашему выбору) любой более поздней
 * версии.
 * Saraff.Tiff.NET распространяется в надежде, что она будет полезной,
 * но БЕЗО ВСЯКИХ ГАРАНТИЙ; даже без неявной гарантии ТОВАРНОГО ВИДА
 * или ПРИГОДНОСТИ ДЛЯ ОПРЕДЕЛЕННЫХ ЦЕЛЕЙ. Подробнее см. в Меньшей Стандартной
 * общественной лицензии GNU.
 * Вы должны были получить копию Меньшей Стандартной общественной лицензии GNU
 * вместе с этой программой. Если это не так, см.
 * <http://www.gnu.org/licenses/>.)
 * 
 * This file is part of Saraff.Tiff.NET.
 * © SARAFF SOFTWARE (Kirnazhytski Andrei), 2014.
 * Saraff.Tiff.NET is free software: you can redistribute it and/or modify
 * it under the terms of the GNU Lesser General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * Saraff.Tiff.NET is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU Lesser General Public License for more details.
 * You should have received a copy of the GNU Lesser General Public License
 * along with Saraff.Tiff.NET. If not, see <http://www.gnu.org/licenses/>.
 * 
 * PLEASE SEND EMAIL TO:  tiff@saraff.ru.
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Collections.ObjectModel;

namespace Saraff.Tiff.Core {

    /// <summary>
    /// TIFF header.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack=1)]
    internal sealed class TiffHeader {

        /// <summary>
        /// Magic number (defines byte order).
        /// </summary>
        [MarshalAs(UnmanagedType.U2)]
        public MagicValues magic;

        /// <summary>
        /// TIFF version number.
        /// </summary>
        public ushort version;

        /// <summary>
        /// Byte offset to first directory.
        /// </summary>
        [TiffHandle]
        public uint dirOffset;
    }

    /// <summary>
    ///  TIFF Image File Directories are comprised of a table of field
    ///  descriptors of the form shown below.  The table is sorted in
    ///  ascending order by tag.  The values associated with each entry are
    ///  disjoint and may appear anywhere in the file (so long as they are
    ///  placed on a word boundary).
    ///  If the value is 4 bytes or less, then it is placed in the offset
    ///  field to save space.  If the value is less than 4 bytes, it is
    ///  left-justified in the offset field.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack=1)]
    internal sealed class TiffDirEntry:ITag {

        /// <summary>
        /// Tag id.
        /// </summary>
        [MarshalAs(UnmanagedType.U2)]
        public TiffTags tag;

        /// <summary>
        /// Data type.
        /// </summary>
        [MarshalAs(UnmanagedType.U2)]
        public TiffDataType type;

        /// <summary>
        /// Number of items; length in spec.
        /// </summary>
        public uint count;

        /// <summary>
        /// Byte offset to field data.
        /// </summary>
        [TiffHandle]
        public uint offset;

        #region ITag Members

        public TiffTags TagId {
            get {
                return this.tag;
            }
        }

        #endregion
    }

    internal enum MagicValues:ushort {
        BigEndian=0x4d4d,
        LittleEndian=0x4949
    }

    /// <summary>
    /// Tag data type information.
    /// </summary>
    /// <remarks>RATIONALs are the ratio of two 32-bit integer values.</remarks>
    internal enum TiffDataType:ushort {

        /// <summary>
        /// Placeholder.
        /// </summary>
        TIFF_NOTYPE=0,

        /// <summary>
        /// 8-bit unsigned integer.
        /// </summary>
        TIFF_BYTE=1,

        /// <summary>
        /// 8-bit bytes w/ last byte null.
        /// </summary>
        TIFF_ASCII=2,

        /// <summary>
        /// 16-bit unsigned integer.
        /// </summary>
        TIFF_SHORT=3,

        /// <summary>
        /// 32-bit unsigned integer.
        /// </summary>
        TIFF_LONG=4,

        /// <summary>
        /// 64-bit unsigned fraction.
        /// </summary>
        TIFF_RATIONAL=5,

        /// <summary>
        /// !8-bit signed integer.
        /// </summary>
        TIFF_SBYTE=6,

        /// <summary>
        /// !8-bit untyped data.
        /// </summary>
        TIFF_UNDEFINED=7,

        /// <summary>
        /// !16-bit signed integer.
        /// </summary>
        TIFF_SSHORT=8,

        /// <summary>
        /// !32-bit signed integer.
        /// </summary>
        TIFF_SLONG=9,

        /// <summary>
        /// !64-bit signed fraction.
        /// </summary>
        TIFF_SRATIONAL=10,

        /// <summary>
        /// !32-bit IEEE floating point.
        /// </summary>
        TIFF_FLOAT=11,

        /// <summary>
        /// !64-bit IEEE floating point.
        /// </summary>
        TIFF_DOUBLE=12,

        /// <summary>
        /// %32-bit unsigned integer (offset).
        /// </summary>
        TIFF_IFD=13
    }

    internal sealed class TiffDataTypeHelper {
        private static Dictionary<TiffDataType, int> _sizeDictionary;
        private static Dictionary<TiffDataType, Type> _typeDictionary;
        private static Dictionary<Type, TiffDataType> _tiffDataTypeDictionary;

        internal static int Sizeof(TiffDataType type) {
            try {
                return TiffDataTypeHelper.SizeDictionary[type];
            } catch(KeyNotFoundException) {
                return 0;
            }
        }

        internal static Type Typeof(TiffDataType type) {
            try {
                return TiffDataTypeHelper.TypeDictionary[type];
            } catch(KeyNotFoundException) {
                return typeof(object);
            }
        }

        internal static TiffDataType TiffDataTypeof(Type type) {
            return TiffDataTypeHelper.TiffDataTypeDictionary[type.IsEnum?Enum.GetUnderlyingType(type):type];
        }

        private static Dictionary<TiffDataType, int> SizeDictionary {
            get {
                if(TiffDataTypeHelper._sizeDictionary==null) {
                    TiffDataTypeHelper._sizeDictionary=new Dictionary<TiffDataType, int> {
                            {TiffDataType.TIFF_NOTYPE,0},
                            {TiffDataType.TIFF_BYTE,1},
                            {TiffDataType.TIFF_ASCII,1},
                            {TiffDataType.TIFF_SHORT,2},
                            {TiffDataType.TIFF_LONG,4},
                            {TiffDataType.TIFF_RATIONAL,8},
                            {TiffDataType.TIFF_SBYTE,1},
                            {TiffDataType.TIFF_UNDEFINED,1},
                            {TiffDataType.TIFF_SSHORT,2},
                            {TiffDataType.TIFF_SLONG,4},
                            {TiffDataType.TIFF_SRATIONAL,8},
                            {TiffDataType.TIFF_FLOAT,4},
                            {TiffDataType.TIFF_DOUBLE,8},
                            {TiffDataType.TIFF_IFD,4}
                        };
                }
                return TiffDataTypeHelper._sizeDictionary;
            }
        }

        private static Dictionary<TiffDataType, Type> TypeDictionary {
            get {
                if(TiffDataTypeHelper._typeDictionary==null) {
                    TiffDataTypeHelper._typeDictionary=new Dictionary<TiffDataType, Type> {
                            {TiffDataType.TIFF_NOTYPE,typeof(object)},
                            {TiffDataType.TIFF_BYTE,typeof(byte)},
                            {TiffDataType.TIFF_ASCII,typeof(char)},
                            {TiffDataType.TIFF_SHORT,typeof(ushort)},
                            {TiffDataType.TIFF_LONG,typeof(uint)},
                            {TiffDataType.TIFF_RATIONAL,typeof(ulong)},
                            {TiffDataType.TIFF_SBYTE,typeof(sbyte)},
                            {TiffDataType.TIFF_UNDEFINED,typeof(byte)},
                            {TiffDataType.TIFF_SSHORT,typeof(short)},
                            {TiffDataType.TIFF_SLONG,typeof(int)},
                            {TiffDataType.TIFF_SRATIONAL,typeof(long)},
                            {TiffDataType.TIFF_FLOAT,typeof(float)},
                            {TiffDataType.TIFF_DOUBLE,typeof(double)},
                            {TiffDataType.TIFF_IFD,typeof(int)}
                        };
                }
                return TiffDataTypeHelper._typeDictionary;
            }
        }

        private static Dictionary<Type, TiffDataType> TiffDataTypeDictionary {
            get {
                if(TiffDataTypeHelper._tiffDataTypeDictionary==null) {
                    TiffDataTypeHelper._tiffDataTypeDictionary=new Dictionary<Type, TiffDataType> {
                        {typeof(byte),TiffDataType.TIFF_BYTE},
                        {typeof(sbyte),TiffDataType.TIFF_SBYTE},
                        {typeof(char),TiffDataType.TIFF_ASCII},
                        {typeof(short),TiffDataType.TIFF_SSHORT},
                        {typeof(ushort),TiffDataType.TIFF_SHORT},
                        {typeof(int),TiffDataType.TIFF_SLONG},
                        {typeof(uint),TiffDataType.TIFF_LONG},
                        {typeof(long),TiffDataType.TIFF_SRATIONAL},
                        {typeof(ulong),TiffDataType.TIFF_RATIONAL},
                        {typeof(float),TiffDataType.TIFF_FLOAT},
                        {typeof(double),TiffDataType.TIFF_DOUBLE},
                        {typeof(TiffHandle),TiffDataType.TIFF_LONG}
                    };
                }
                return TiffDataTypeHelper._tiffDataTypeDictionary;
            }
        }
    }

    /// <summary>
    /// TIFF Tag Definitions.
    /// </summary>
    public enum TiffTags:ushort {
        NewSubfileType=254, /* subfile data descriptor */
        SubfileType=255, /* +kind of data in subfile */
        ImageWidth=256, /* image width in pixels */
        ImageLength=257, /* image height in pixels */
        BitsPerSample=258, /* bits per channel (sample) */
        Compression=259, /* data compression technique */
        PhotometricInterpretation=262, /* photometric interpretation */
        Threshholding=263, /* +thresholding used on data */
        CellWidth=264, /* +dithering matrix width */
        CellLength=265, /* +dithering matrix height */
        FillOrder=266, /* data order within a byte */
        DocumentName=269, /* name of doc. image is from */
        ImageDescription=270, /* info about image */
        Make=271, /* scanner manufacturer name */
        Model=272, /* scanner model name/number */
        StripOffsets=273, /* offsets to data strips */
        Orientation=274, /* +image orientation */
        SamplesPerPixel=277, /* samples per pixel */
        RowsPerStrip=278, /* rows per strip of data */
        StripByteCounts=279, /* bytes counts for strips */
        MinSampleValue=280, /* +minimum sample value */
        MaxSampleValue=281, /* +maximum sample value */
        XResolution=282, /* pixels/resolution in x */
        YResolution=283, /* pixels/resolution in y */
        PlanarConfiguration=284, /* storage organization */
        PageName=285, /* page name image is from */
        XPosition=286, /* x page offset of image lhs */
        YPosition=287, /* y page offset of image lhs */
        FreeOffsets=288, /* +byte offset to free block */
        FreeByteCounts=289, /* +sizes of free blocks */
        GrayResponseUnit=290, /* $gray scale curve accuracy */
        GrayResponseCurve=291, /* $gray scale response curve */
        T4Options=292, /* 32 flag bits */
        T6Options=292, /* TIFF 6.0 proper name alias */
        ResolutionUnit=296, /* units of resolutions */
        PageNumber=297, /* page numbers of multi-page */
        ColorResponseUnit=300, /* $color curve accuracy */
        TransferFunction=301, /* !colorimetry info */
        Software=305, /* name & release */
        DateTime=306, /* creation date and time */
        Artist=315, /* creator of image */
        HostComputer=316, /* machine where created */
        Predictor=317, /* prediction scheme w/ LZW */
        WhitePoint=318, /* image white point */
        PrimaryChromaticities=319, /* !primary chromaticities */
        ColorMap=320, /* RGB map for pallette image */
        HalftoneHints=321, /* !highlight+shadow info */
        TileWidth=322, /* !tile width in pixels */
        TileLength=323, /* !tile height in pixels */
        TileOffsets=324, /* !offsets to data tiles */
        TileByteCounts=325, /* !byte counts for tiles */
        BadFaxLines=326, /* lines w/ wrong pixel count */
        CleanFaxData=327, /* regenerated line info */
        ConsecutiveBadFaxLines=328, /* max consecutive bad lines */
        SubIfd=330, /* subimage descriptors */
        InkSet=332, /* !inks in separated image */
        InkNames=333, /* !ascii names of inks */
        NumberOfInks=334, /* !number of inks */
        DotRange=336, /* !0% and 100% dot codes */
        TargetPrinter=337, /* !separation target */
        ExtraSamples=338, /* !info about extra samples */
        SampleFormat=339, /* !data sample format */
        SMinSampleValue=340, /* !variable MinSampleValue */
        SMaxSampleValue=341, /* !variable MaxSampleValue */
        TransferRange=342,
        ClipPath=343, /* %ClipPath [Adobe TIFF technote 2] */
        XClipPathUnits=344, /* %XClipPathUnits [Adobe TIFF technote 2] */
        YClipPathUnits=345, /* %YClipPathUnits [Adobe TIFF technote 2] */
        Indexed=346, /* %Indexed [Adobe TIFF Technote 3] */
        JpegTables=347, /* %JPEG table stream */
        OpiProxy=351, /* %OPI Proxy [Adobe TIFF technote] */

        /*
        * Tags 512-521 are obsoleted by Technical Note #2 which specifies a
        * revised JPEG-in-TIFF scheme.
        */
        JPEGProc=512, /* !JPEG processing algorithm */
        JPEGInterchangeFormat=513, /* !pointer to SOI marker */
        JPEGInterchangeFormatLngth=514, /* !JFIF stream length */
        JPEGRestartInterval=515, /* !restart interval length */
        JPEGLosslessPredictors=517, /* !lossless proc predictor */
        JPEGPointTransforms=518, /* !lossless point transform */
        JPEGQTables=519, /* !Q matrice offsets */
        JPEGDCTables=520, /* !DCT table offsets */
        JPEGACTables=521, /* !AC coefficient offsets */
        YCbCrCoefficients=529, /* !RGB -> YCbCr transform */
        YCbCrSubSampling=530, /* !YCbCr subsampling factors */
        YCbCrPositioning=531, /* !subsample positioning */
        ReferenceBlackWhite=532, /* !colorimetry info */
        XmlPacket=700, /* %XML packet [Adobe XMP Specification, January 2004 */
        OpiImageId=32781, /* %OPI ImageID [Adobe TIFF technote] */

        /* tags 32952-32956 are private tags registered to Island Graphics */
        REFPTS=32953, /* image reference points */
        REGIONTACKPOINT=32954, /* region-xform tack point */
        REGIONWARPCORNERS=32955, /* warp quadrilateral */
        REGIONAFFINE=32956, /* affine transformation mat */

        /* tags 32995-32999 are private tags registered to SGI */
        MATTEING=32995, /* $use ExtraSamples */
        DATATYPE=32996, /* $use SampleFormat */
        IMAGEDEPTH=32997, /* z depth of image */
        TILEDEPTH=32998, /* z depth/data tile */

        /* tags 33300-33309 are private tags registered to Pixar */
        /*
        * TIFFTAG_PIXAR_IMAGEFULLWIDTH and TIFFTAG_PIXAR_IMAGEFULLLENGTH
        * are set when an image has been cropped out of a larger image.  
        * They reflect the size of the original uncropped image.
        * The TIFFTAG_XPOSITION and TIFFTAG_YPOSITION can be used
        * to determine the position of the smaller image in the larger one.
        */
        PIXAR_IMAGEFULLWIDTH=33300, /* full image size in x */
        PIXAR_IMAGEFULLLENGTH=33301, /* full image size in y */

        /* Tags 33302-33306 are used to identify special image modes and data
        * used by Pixar's texture formats.
        */
        PIXAR_TEXTUREFORMAT=33302, /* texture map format */
        PIXAR_WRAPMODES=33303, /* s & t wrap modes */
        PIXAR_FOVCOT=33304, /* cotan(fov) for env. maps */
        PIXAR_MATRIX_WORLDTOSCREEN=33305,
        PIXAR_MATRIX_WORLDTOCAMERA=33306,

        /* tag 33405 is a private tag registered to Eastman Kodak */
        WRITERSERIALNUMBER=33405, /* device serial number */

        /* tag 33432 is listed in the 6.0 spec w/ unknown ownership */
        Copyright=33432, /* copyright string */

        /* IPTC TAG from RichTIFF specifications */
        RICHTIFFIPTC=33723,

        /* 34016-34029 are reserved for ANSI IT8 TIFF/IT <dkelly@apago.com) */
        IT8SITE=34016, /* site name */
        IT8COLORSEQUENCE=34017, /* color seq. [RGB,CMYK,etc] */
        IT8HEADER=34018, /* DDES Header */
        IT8RASTERPADDING=34019, /* raster scanline padding */
        IT8BITSPERRUNLENGTH=34020, /* # of bits in short run */
        IT8BITSPEREXTENDEDRUNLENGTH=34021,/* # of bits in long run */
        IT8COLORTABLE=34022, /* LW colortable */
        IT8IMAGECOLORINDICATOR=34023, /* BP/BL image color switch */
        IT8BKGCOLORINDICATOR=34024, /* BP/BL bg color switch */
        IT8IMAGECOLORVALUE=34025, /* BP/BL image color value */
        IT8BKGCOLORVALUE=34026, /* BP/BL bg color value */
        IT8PIXELINTENSITYRANGE=34027, /* MP pixel intensity value */
        IT8TRANSPARENCYINDICATOR=34028, /* HC transparency switch */
        IT8COLORCHARACTERIZATION=34029, /* color character. table */
        IT8HCUSAGE=34030, /* HC usage indicator */
        IT8TRAPINDICATOR=34031, /* Trapping indicator (untrapped=0, trapped=1) */
        IT8CMYKEQUIVALENT=34032, /* CMYK color equivalents */

        /* tags 34232-34236 are private tags registered to Texas Instruments */
        FRAMECOUNT=34232, /* Sequence Frame Count */

        /* tag 34377 is private tag registered to Adobe for PhotoShop */
        PHOTOSHOP=34377,

        /* tags 34665, 34853 and 40965 are documented in EXIF specification */
        EXIFIFD=34665, /* Pointer to EXIF private directory */

        /* tag 34750 is a private tag registered to Adobe? */
        ICCPROFILE=34675, /* ICC profile data */

        /* tag 34750 is a private tag registered to Pixel Magic */
        JBIGOPTIONS=34750, /* JBIG options */
        GPSIFD=34853, /* Pointer to GPS private directory */

        /* tags 34908-34914 are private tags registered to SGI */
        FAXRECVPARAMS=34908, /* encoded Class 2 ses. parms */
        FAXSUBADDRESS=34909, /* received SubAddr string */
        FAXRECVTIME=34910, /* receive time (secs) */
        FAXDCS=34911, /* encoded fax ses. params, Table 2/T.30 */

        /* tags 37439-37443 are registered to SGI <gregl@sgi.com> */
        STONITS=37439, /* Sample value to Nits */

        /* tag 34929 is a private tag registered to FedEx */
        FEDEX_EDR=34929, /* unknown use */
        INTEROPERABILITYIFD=40965, /* Pointer to Interoperability private directory */

        /* Adobe Digital Negative (DNG) format tags */
        DNGVERSION=50706, /* &DNG version number */
        DNGBACKWARDVERSION=50707, /* &DNG compatibility version */
        UNIQUECAMERAMODEL=50708, /* &name for the camera model */
        LOCALIZEDCAMERAMODEL=50709, /* &localized camera model name */
        CFAPLANECOLOR=50710, /* &CFAPattern->LinearRaw space mapping */
        CFALAYOUT=50711, /* &spatial layout of the CFA */
        LINEARIZATIONTABLE=50712, /* &lookup table description */
        BLACKLEVELREPEATDIM=50713, /* &repeat pattern size for the BlackLevel tag */
        BLACKLEVEL=50714, /* &zero light encoding level */
        BLACKLEVELDELTAH=50715, /* &zero light encoding level differences (columns) */
        BLACKLEVELDELTAV=50716, /* &zero light encoding level differences (rows) */
        WHITELEVEL=50717, /* &fully saturated encoding level */
        DEFAULTSCALE=50718, /* &default scale factors */
        DEFAULTCROPORIGIN=50719, /* &origin of the final image area */
        DEFAULTCROPSIZE=50720, /* &size of the final image area */
        COLORMATRIX1=50721, /* &XYZ->reference color space transformation matrix 1 */
        COLORMATRIX2=50722, /* &XYZ->reference color space transformation matrix 2 */
        CAMERACALIBRATION1=50723, /* &calibration matrix 1 */
        CAMERACALIBRATION2=50724, /* &calibration matrix 2 */
        REDUCTIONMATRIX1=50725, /* &dimensionality reduction matrix 1 */
        REDUCTIONMATRIX2=50726, /* &dimensionality reduction matrix 2 */
        ANALOGBALANCE=50727, /* &gain applied the stored raw values*/
        ASSHOTNEUTRAL=50728, /* &selected white balance in linear reference space */
        ASSHOTWHITEXY=50729, /* &selected white balance in x-y chromaticity coordinates */
        BASELINEEXPOSURE=50730, /* &how much to move the zero point */
        BASELINENOISE=50731, /* &relative noise level */
        BASELINESHARPNESS=50732, /* &relative amount of sharpening */
        BAYERGREENSPLIT=50733, /* &how closely the values of the green pixels in the blue/green rows track the values of the green pixels in the red/green rows */
        LINEARRESPONSELIMIT=50734, /* &non-linear encoding range */
        CAMERASERIALNUMBER=50735, /* &camera's serial number */
        LENSINFO=50736, /* info about the lens */
        CHROMABLURRADIUS=50737, /* &chroma blur radius */
        ANTIALIASSTRENGTH=50738, /* &relative strength of the camera's anti-alias filter */
        SHADOWSCALE=50739, /* &used by Adobe Camera Raw */
        DNGPRIVATEDATA=50740, /* &manufacturer's private data */
        MAKERNOTESAFETY=50741, /* &whether the EXIF MakerNote tag is safe to preserve along with the rest of the EXIF data */
        CALIBRATIONILLUMINANT1=50778, /* &illuminant 1 */
        CALIBRATIONILLUMINANT2=50779, /* &illuminant 2 */
        BESTQUALITYSCALE=50780, /* &best quality multiplier */
        RAWDATAUNIQUEID=50781, /* &unique identifier for the raw image data */
        ORIGINALRAWFILENAME=50827, /* &file name of the original raw file */
        ORIGINALRAWFILEDATA=50828, /* &contents of the original raw file */
        ACTIVEAREA=50829, /* &active (non-masked) pixels of the sensor */
        MASKEDAREAS=50830, /* &list of coordinates of fully masked pixels */
        ASSHOTICCPROFILE=50831, /* &these two tags used to */
        ASSHOTPREPROFILEMATRIX=50832, /* map cameras's color space into ICC profile space */
        CURRENTICCPROFILE=50833, /* & */
        CURRENTPREPROFILEMATRIX=50834, /* & */

        /* tag 65535 is an undefined tag used by Eastman Kodak */
        DCSHUESHIFTVALUES=65535   /* hue shift correction data */
    }

    [TiffTag(TiffTags.NewSubfileType)]
    public enum TiffNewSubFileType:uint {
        REDUCEDIMAGE=0x1, /* reduced resolution version */
        PAGE=0x2, /* one page of many */
        MASK=0x4, /* transparency mask */
    }

    [TiffTag(TiffTags.SubfileType)]
    public enum TiffSubFileType:ushort {
        IMAGE=1, /* full resolution image data */
        REDUCEDIMAGE=2, /* reduced size image data */
        PAGE=3, /* one page of many */
    }

    [TiffTag(TiffTags.Compression)]
    public enum TiffCompression:ushort {
        NONE=1, /* dump mode */
        CCITTRLE=2, /* CCITT modified Huffman RLE */
        CCITTFAX3=3, /* CCITT Group 3 fax encoding */
        CCITT_T4=3, /* CCITT T.4 (TIFF 6 name) */
        CCITTFAX4=4, /* CCITT Group 4 fax encoding */
        CCITT_T6=4, /* CCITT T.6 (TIFF 6 name) */
        LZW=5, /* Lempel-Ziv  & Welch */
        OJPEG=6, /* !6.0 JPEG */
        JPEG=7, /* %JPEG DCT compression */
        NEXT=32766, /* NeXT 2-bit RLE */
        CCITTRLEW=32771, /* #1 w/ word alignment */
        PACKBITS=32773, /* Macintosh RLE */
        THUNDERSCAN=32809, /* ThunderScan RLE */

        /* codes 32895-32898 are reserved for ANSI IT8 TIFF/IT <dkelly@apago.com) */
        IT8CTPAD=32895, /* IT8 CT w/padding */
        IT8LW=32896, /* IT8 Linework RLE */
        IT8MP=32897, /* IT8 Monochrome picture */
        IT8BL=32898, /* IT8 Binary line art */

        /* compression codes 32908-32911 are reserved for Pixar */
        PIXARFILM=32908, /* Pixar companded 10bit LZW */
        PIXARLOG=32909, /* Pixar companded 11bit ZIP */
        DEFLATE=32946, /* Deflate compression */
        ADOBE_DEFLATE=8, /* Deflate compression, as recognized by Adobe */

        /* compression code 32947 is reserved for Oceana Matrix <dev@oceana.com> */
        DCS=32947, /* Kodak DCS encoding */
        JBIG=34661, /* ISO JBIG */
        SGILOG=34676, /* SGI Log Luminance RLE */
        SGILOG24=34677, /* SGI Log 24-bit packed */
        JP2000=34712, /* Leadtools JPEG2000 */
    }

    [TiffTag(TiffTags.PhotometricInterpretation)]
    public enum TiffPhotoMetric:ushort {
        WhiteIsZero=0, /* min value is white */
        BlackIsZero=1, /* min value is black */
        RGB=2, /* RGB color model */
        Palette=3, /* color map indexed */
        MASK=4, /* $holdout mask */
        SEPARATED=5, /* !color separations */
        YCbCr=6, /* !CCIR 601 */
        CIELab=8, /* !1976 CIE L*a*b* */
        ICCLab=9, /* ICC L*a*b* [Adobe TIFF Technote 4] */
        ITULab=10, /* ITU L*a*b* */
        LOGL=32844, /* CIE Log2(L) */
        LOGLUV=32845, /* CIE Log2(L) (u',v') */
    }

    [TiffTag(TiffTags.Threshholding)]
    public enum TiffThreshHolding:ushort {
        BILEVEL=1, /* b&w art scan */
        HALFTONE=2, /* or dithered scan */
        ERRORDIFFUSE=3, /* usually floyd-steinberg */
    }

    [TiffTag(TiffTags.FillOrder)]
    public enum TiffFillOrder:ushort {
        MSB2LSB=1, /* most significant -> least */
        LSB2MSB=2, /* least significant -> most */
    }

    [TiffTag(TiffTags.Orientation)]
    public enum TiffOrientation:ushort {
        TOPLEFT=1, /* row 0 top, col 0 lhs */
        TOPRIGHT=2, /* row 0 top, col 0 rhs */
        BOTRIGHT=3, /* row 0 bottom, col 0 rhs */
        BOTLEFT=4, /* row 0 bottom, col 0 lhs */
        LEFTTOP=5, /* row 0 lhs, col 0 top */
        RIGHTTOP=6, /* row 0 rhs, col 0 top */
        RIGHTBOT=7, /* row 0 rhs, col 0 bottom */
        LEFTBOT=8, /* row 0 lhs, col 0 bottom */
    }

    [TiffTag(TiffTags.PlanarConfiguration)]
    public enum TiffPlanarConfig:ushort {
        CONTIG=1, /* single image plane */
        SEPARATE=2, /* separate planes of data */
    }

    [TiffTag(TiffTags.GrayResponseUnit)]
    public enum TiffGrayResponseUnit:ushort {
        _10S=1, /* tenths of a unit */
        _100S=2, /* hundredths of a unit */
        _1000S=3, /* thousandths of a unit */
        _10000S=4, /* ten-thousandths of a unit */
        _100000S=5, /* hundred-thousandths */
    }

    [Flags]
    public enum TiffGroup4Options:uint {
        _2DENCODING=0x1, /* 2-dimensional coding */
        UNCOMPRESSED=0x2, /* data not compressed */
        FILLBITS=0x4, /* fill to byte boundary */
    }

    [Flags]
    public enum TiffGroup6Options:uint {
        UNCOMPRESSED=0x2, /* data not compressed */
    }

    [TiffTag(TiffTags.ResolutionUnit)]
    public enum TiffResolutionUnit:ushort {
        NONE=1, /* no meaningful units */
        INCH=2, /* english */
        CENTIMETER=3, /* metric */
    }

    [TiffTag(TiffTags.ColorResponseUnit)]
    public enum TiffColorResponseUnit:ushort {
        _10S=1, /* tenths of a unit */
        _100S=2, /* hundredths of a unit */
        _1000S=3, /* thousandths of a unit */
        _10000S=4, /* ten-thousandths of a unit */
        _100000S=5, /* hundred-thousandths */
    }

    [TiffTag(TiffTags.Predictor)]
    public enum TiffPredictor:ushort {
        NONE=1, /* no prediction scheme used */
        HORIZONTAL=2, /* horizontal differencing */
        FLOATINGPOINT=3, /* floating point predictor */
    }

    internal enum TiffCleanFaxData {
        CLEAN=0, /* no errors detected */
        REGENERATED=1, /* receiver regenerated lines */
        UNCLEAN=2, /* uncorrected errors exist */
    }

    [TiffTag(TiffTags.InkSet)]
    public enum TiffInkSet:ushort {
        CMYK=1, /* !cyan-magenta-yellow-black color */
        MULTIINK=2, /* !multi-ink or hi-fi color */
    }

    [TiffTag(TiffTags.ExtraSamples)]
    public enum TiffExtraSamples:ushort {
        UNSPECIFIED=0, /* !unspecified data */
        ASSOCALPHA=1, /* !associated alpha data */
        UNASSALPHA=2, /* !unassociated alpha data */
    }

    [TiffTag(TiffTags.SampleFormat)]
    public enum TiffSampleFormat:ushort {
        UINT=1, /* !unsigned integer data */
        INT=2, /* !signed integer data */
        IEEEFP=3, /* !IEEE floating point data */
        VOID=4, /* !untyped data */
        COMPLEXINT=5, /* !complex signed int */
        COMPLEXIEEEFP=6, /* !complex ieee floating */
    }

    [TiffTag(TiffTags.JPEGProc)]
    public enum TiffJpegProc:ushort {
        BASELINE=1, /* !baseline sequential */
        LOSSLESS=14, /* !Huffman coded lossless */
    }

    internal enum TiffYcbcrPositionIN {
        CENTERED=1, /* !as in PostScript Level 2 */
        COSITED=2, /* !as in CCIR 601-1 */
    }

}
