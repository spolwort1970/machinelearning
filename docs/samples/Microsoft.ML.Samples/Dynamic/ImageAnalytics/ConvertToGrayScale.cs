﻿using System.IO;
using Microsoft.ML.Data;

namespace Microsoft.ML.Samples.Dynamic
{
    public class ConvertToGrayscaleExample
    {
        // Sample that loads images from the file system, and converts them to grayscale. 
        public static void ConvertToGrayscale()
        {
            var mlContext = new MLContext();

            // Downloading a few images, and an images.tsv file, which contains a list of the files from the dotnet/machinelearning/test/data/images/.
            // If you inspect the fileSystem, after running this line, an "images" folder will be created, containing 4 images, and a .tsv file
            // enumerating the images. 
            var imagesDataFile = SamplesUtils.DatasetUtils.DownloadImages();

            // Preview of the content of the images.tsv file
            //
            // imagePath    imageType
            // tomato.bmp   tomato
            // banana.jpg   banana
            // hotdog.jpg   hotdog
            // tomato.jpg   tomato

            var data = mlContext.Data.CreateTextLoader(new TextLoader.Arguments()
            {
                Columns = new[]
                {
                        new TextLoader.Column("ImagePath", DataKind.TX, 0),
                        new TextLoader.Column("Name", DataKind.TX, 1),
                 }
            }).Read(imagesDataFile);

            var imagesFolder = Path.GetDirectoryName(imagesDataFile);
            // Image loading pipeline. 
            var pipeline = mlContext.Transforms.LoadImages(imagesFolder, ("ImageObject", "ImagePath"))
                           .Append(mlContext.Transforms.ConvertToGrayscale(("Grayscale", "ImageObject")));

            var transformedData = pipeline.Fit(data).Transform(data);

            // The transformedData IDataView contains the loaded images column, and the grayscaled column.
            // Preview of the transformedData
            var transformedDataPreview = transformedData.Preview();

            // Preview of the content of the images.tsv file
            // The actual images, in the Grayscale column are of type System.Drawing.Bitmap.
            //
            // ImagePath    Name        ImageObject                   Grayscale
            // tomato.bmp   tomato      {System.Drawing.Bitmap}     {System.Drawing.Bitmap}
            // banana.jpg   banana      {System.Drawing.Bitmap}     {System.Drawing.Bitmap}
            // hotdog.jpg   hotdog      {System.Drawing.Bitmap}     {System.Drawing.Bitmap}
            // tomato.jpg   tomato      {System.Drawing.Bitmap}     {System.Drawing.Bitmap}

        }
    }
}
