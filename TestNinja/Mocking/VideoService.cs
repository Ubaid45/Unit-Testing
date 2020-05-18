﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using Newtonsoft.Json;

namespace TestNinja.Mocking
{
    public class VideoService
    {
        private readonly IVideoRepository _videoRepository;
        private readonly IFileReader _fileReader;
        public VideoService(IFileReader fileReader = null, IVideoRepository videoRepository = null)
        {
            _videoRepository = videoRepository ?? new VideoRepository();
            _fileReader = fileReader ?? new FileReader();
        }
        public string ReadVideoTitle()
        {
            var str = _fileReader.Read(("video.txt")); 
            var video = JsonConvert.DeserializeObject<Video>(str);
            if (video == null)
                return "Error parsing the video.";
            return video.Title;
        }

        public string GetUnprocessedVideosAsCsv()
        {
            var videoIds = new List<int>();
            
            foreach (var v in _videoRepository.GetOnProcessedVideos())
                    videoIds.Add(v.Id);

            return String.Join(",", videoIds);
            }
        }

    public class Video
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public bool IsProcessed { get; set; }
    }

    public class VideoContext : DbContext
    {
        public DbSet<Video> Videos { get; set; }
    }
}