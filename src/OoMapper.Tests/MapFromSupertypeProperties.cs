﻿using Xunit;

namespace OoMapper.Tests
{
    public class MapFromSupertypeProperties : TestBase
    {
        [Fact]
        public void TestAutomapping()
        {
            Mapper.CreateMap<Source, Destination>();
            var source = new Source
                             {
                                 Value = 100500,
                             };
            Destination destination = Mapper.Map<Source, Destination>(source);
            Assert.Equal(100500, destination.Value);
        }

        [Fact]
        public void TestExplicitMapping()
        {
            Mapper.CreateMap<Source, Destination>()
                .ForMember(x => x.Value, opt => opt.MapFrom(x => x.Value));

            var source = new Source
                             {
                                 Value = 100500,
                             };
            Destination destination = Mapper.Map<Source, Destination>(source);
            Assert.Equal(100500, destination.Value);
        }

        #region Nested type: Destination

        public class Destination
        {
            public int Value { get; set; }
        }

        #endregion

        #region Nested type: Source

        public class Source : SourceParent
        {
        }

        #endregion

        #region Nested type: SourceParent

        public class SourceParent
        {
            public int Value { get; set; }
        }

        #endregion
    }
}