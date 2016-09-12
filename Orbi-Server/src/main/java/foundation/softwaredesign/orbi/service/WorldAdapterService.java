package foundation.softwaredesign.orbi.service;

import foundation.softwaredesign.orbi.model.GeoPosition;
import foundation.softwaredesign.orbi.model.Position;

import javax.enterprise.context.RequestScoped;

/**
 * @author Lucas Reeh <lr86gm@gmail.com>
 */
@RequestScoped
public class WorldAdapterService {

    private final static Integer ZOOM = 18;
    private final static Integer NUM_TILES = (int)Math.pow(2, ZOOM);
    private final static Integer TILE_SIZE = 256;
    private final static Double PIXEL_PER_LONG_DEGREE = TILE_SIZE / 360.0d;
    private final static Double PIXEL_PER_LONG_RADIAN = TILE_SIZE / (2d * Math.PI);

    public GeoPosition toGeo(Position original, GeoPosition center) {
        Position position = new Position();
        // get virtual center
        Position centerVirtual = fromGeoToVirtual(center);

        // translate
        position.setZ(centerVirtual.getZ() + original.getZ());
        position.setX(centerVirtual.getX() + original.getX());
        position.setY(original.getY());

        // to real
        return fromVirtualToGeo(position);
    }

    private GeoPosition fromVirtualToGeo(Position original) {
        // copy
        GeoPosition position = new GeoPosition();

        // virtual
        position.setLatitude(original.getZ() * 2d);
        position.setLongitude(original.getX() * 2d);
        position.setAltitude(original.getY());

        // pixel
        position.setLatitude(position.getLatitude() / NUM_TILES);
        position.setLongitude(position.getLongitude() / NUM_TILES);

        // projection
        position.setLongitude(position.getLongitude() / PIXEL_PER_LONG_DEGREE);
        double latRadians = position.getLatitude() / PIXEL_PER_LONG_RADIAN;
        position.setLatitude(radiansToDegrees(2d * Math.atan(Math.exp(latRadians)) - Math.PI / 2d));

        return position;
    }

    public Position toVirtual(GeoPosition original, GeoPosition center) {
        GeoPosition position = new GeoPosition();
        // translate
        position.setLatitude(original.getLatitude() - center.getLatitude());
        position.setLongitude(original.getLongitude() - center.getLongitude());
        position.setAltitude(original.getAltitude());

        return fromGeoToVirtual(position);
    }

    private Position fromGeoToVirtual(GeoPosition original) {
        Position position = new Position();

        // projection
        // Truncating to 0.9999 effectively limits latitude to 89.189. This is
        // about a third of a tile past the edge of the world tile.
        Double siny = Math.sin(degreesToRadians(original.getLatitude()));
        position.setZ(-((.5d * Math.log((1d + siny) / (1d - siny)) * -PIXEL_PER_LONG_RADIAN)));
        position.setX(original.getLongitude() * PIXEL_PER_LONG_DEGREE);
        position.setY(original.getAltitude());

        // pixel
        position.setZ(position.getZ() * NUM_TILES);
        position.setX(position.getX() * NUM_TILES);

        // virtual
        position.setZ(position.getZ() / 2d);
        position.setX(position.getX() / 2d);

        return position;
    }

    private Double degreesToRadians(Double deg)
    {
        return deg * (Math.PI / 180d);
    }

    private Double radiansToDegrees(Double rad)
    {
        return rad / (Math.PI / 180d);
    }
}
