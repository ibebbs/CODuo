namespace CODuo.Common
{
    public class Weather
    {
        /// <summary>
        /// Screen Air Temperature
        /// </summary>
        /// <remarks>
        /// degrees Celsius
        /// </remarks>
        public float Temperature { get; set; }

        /// <summary>
        /// 10m Wind Speed
        /// </summary>
        /// <remarks>
        /// metres per second
        /// </remarks>
        public float WindSpeed { get; set; }

        /// <summary>
        /// 10m Wind From Direction
        /// </summary>
        /// <remarks>
        /// degrees
        /// </remarks>
        public float WindDirection { get; set; }

        /// <summary>
        /// Screen Relative Humidity
        /// </summary>
        /// <remarks>
        /// percentage
        /// </remarks>
        public float Humidity { get; set; }

        /// <summary>
        /// Mean Sea Level Pressure
        /// </summary>
        /// <remarks>
        /// pascals
        /// </remarks>
        public int Pressure { get; set; }

        /// <summary>
        /// Visibility
        /// </summary>
        /// <remarks>
        /// metres
        /// </remarks>
        public int Visibility { get; set; }

        /// <summary>
        /// Probability of Precipitation
        /// </summary>
        /// <remarks>
        /// percentage
        /// </remarks>
        public int PrecipitationProbability { get; set; }

        /// <summary>
        /// Precipitation Rate
        /// </summary>
        /// <remarks>
        /// millimetres per hour
        /// </remarks>
        public float PrecipitationRate { get; set; }
    }
}
