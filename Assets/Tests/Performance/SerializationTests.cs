using System;
using System.Collections.Generic;
using System.IO;
using Bridge.Modules.Serialization;
using Bridge.Services.AssetService.Caching;
using NUnit.Framework;
using Unity.PerformanceTesting;
using UnityEngine;

namespace Tests.Performance
{
    public partial class SerializationTests
    {
        private readonly ISerializer _serializer = new Serializer();

        [Test, Performance]
        public void SerializeCacheDataWithProtobuf()
        {
            var testData = GetTestCacheData();

            Measure.Method(() => { _serializer.SerializeProtobuf(testData); }).WarmupCount(10).MeasurementCount(10)
                .IterationsPerMeasurement(5).Run();
        }

        [Test, Performance]
        public void SerializeCacheDataWithJsonNet()
        {
            var testData = GetTestCacheData();

            Measure.Method(() => { _serializer.SerializeToJson(testData); }).WarmupCount(10).MeasurementCount(10)
                .IterationsPerMeasurement(5).Run();
        }
        
        [Test, Performance]
        public void DeserializeCacheDataWithProtobuf()
        {
            var testData = GetTestCacheData();

            var bytes = _serializer.SerializeProtobuf(testData);
            var filePath = Application.persistentDataPath + "/Tests/Performance/cacheDataProtobuf";
            var directory = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);
            File.WriteAllBytes(filePath, bytes);

            Measure.Method(() =>
            {
                using (var stream = File.OpenRead(filePath))
                {
                    _serializer.DeserializeProtobuf<List<FileData>>(stream);
                }
            }).WarmupCount(10).MeasurementCount(10).IterationsPerMeasurement(5).Run();
        }

        [Test, Performance]
        public void DeserializeCacheDataWithJsonNet()
        {
            var testData = GetTestCacheData();

            var testDataJson = _serializer.SerializeToJson(testData);
            var filePath = Application.persistentDataPath + "/Tests/Performance/cacheDataJson";
            var directory = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            File.WriteAllText(filePath, testDataJson);

            Measure.Method(() =>
            {
                var json = File.ReadAllText(filePath);
                _serializer.DeserializeJson<List<FileData>>(json);
            }).WarmupCount(10).MeasurementCount(10).IterationsPerMeasurement(5).Run();
        }

        private List<FileData> GetTestCacheData()
        {
            var testData = new List<FileData>();
            for (int i = 0; i < 500; i++)
            {
                testData.Add(new FileData()
                {
                    AssetTypeName = "SomeTestType",
                    DownloadedDateUTC = DateTime.Now,
                    LastUsedDateUTC = DateTime.Now,
                    Path = "Cache/SubFolder/AnotherSubfolder/Platform/file.txt",
                    UsingCount = 10,
                    Version = Guid.NewGuid().ToString()
                });
            }

            return testData;
        }
    }
}