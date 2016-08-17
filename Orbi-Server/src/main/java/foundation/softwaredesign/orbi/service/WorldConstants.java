package foundation.softwaredesign.orbi.service;

/**
 * @author Lucas Reeh <lr86gm@gmail.com>
 */
public class WorldConstants {

    public static final Double AVG_EARTH_CIRCUMFERENCE = 40036000.0;
    public static final Double LATITUDE_SCALE = 180.0 / AVG_EARTH_CIRCUMFERENCE;
    public static final Double LONGITUDE_SCALE = 360.0 / AVG_EARTH_CIRCUMFERENCE;
    public static final Double LATITUDE_SCALE_REVERSE = AVG_EARTH_CIRCUMFERENCE / 180.0;
    public static final Double LONGITUDE_SCALE_REVERSE = AVG_EARTH_CIRCUMFERENCE / 360.0;

}
