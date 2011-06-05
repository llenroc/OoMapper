﻿using Xunit;

namespace OoMapper.Tests
{
    public class MapArray
    {
        [Fact]
        public void Test()
        {
            Mapper.CreateMap<SourceChild, DestinationChild>();
            Mapper.CreateMap<Source, Destination>();

            var source = new Source
                             {
                                 Children = new[]
                                                {
                                                    new SourceChild
                                                        {
                                                            A = "hello world"
                                                        },
                                                }
                             };

            Destination destination = Mapper.Map<Source, Destination>(source);
            Assert.Equal(1, destination.Children.Length);
        }

        #region Nested type: Destination

        private class Destination
        {
            public DestinationChild[] Children { get; set; }
        }

        #endregion

        #region Nested type: DestinationChild

        private class DestinationChild
        {
            public string A { get; set; }
        }

        #endregion

        #region Nested type: Source

        private class Source
        {
            public SourceChild[] Children { get; set; }
        }

        #endregion

        #region Nested type: SourceChild

        private class SourceChild
        {
            public string A { get; set; }
        }

        #endregion
    }
}
