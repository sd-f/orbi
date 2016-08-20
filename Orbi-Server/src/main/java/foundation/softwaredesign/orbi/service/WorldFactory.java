package foundation.softwaredesign.orbi.service;

import foundation.softwaredesign.orbi.model.GameObject;
import foundation.softwaredesign.orbi.model.GeoPosition;
import foundation.softwaredesign.orbi.persistence.repo.GameObjectRepository;

import javax.enterprise.context.RequestScoped;
import javax.inject.Inject;
import java.util.HashMap;
import java.util.Map;

/**
 * @author Lucas Reeh <lr86gm@gmail.com>
 */
@RequestScoped
public class WorldFactory {

    @Inject
    GameObjectRepository gameObjectRepository;

    @Inject
    ElevationService elevationService;

    Map<String, GeoPosition> initialObjects;

    public void reset() {
        initialObjects = new HashMap<>();
        Double y0 = 47.0680; // N
        Double y2 = 47.0676; // S

        Double x0 = 15.5554; // E
        Double x2 = 15.5550; // W

        Double x1 = 15.5552;
        Double y1 = 47.0678;

        initialObjects.put("test_calibration_N__1", new GeoPosition(x1, 0.5, y0)); // N
        initialObjects.put("test_calibration_N__2", new GeoPosition(x1, 1.5, y0)); // N
        initialObjects.put("test_calibration_N__3", new GeoPosition(x1, 2.5, y0)); // N
        initialObjects.put("test_calibration_N__4", new GeoPosition(x1, 3.5, y0)); // N
        initialObjects.put("test_calibration_NE_1", new GeoPosition(x0, 0.5, y0)); // NE
        initialObjects.put("test_calibration_E__1", new GeoPosition(x0, 0.5, y1)); // E
        initialObjects.put("test_calibration_E__2", new GeoPosition(x0, 1.5, y1)); // E
        initialObjects.put("test_calibration_E__3", new GeoPosition(x0, 2.5, y1)); // E
        initialObjects.put("test_calibration_SE_1", new GeoPosition(x0, 0.5, y2)); // SE
        initialObjects.put("test_calibration_S__1", new GeoPosition(x1, 0.5, y2)); // S
        initialObjects.put("test_calibration_S__2", new GeoPosition(x1, 1.5, y2)); // S
        initialObjects.put("test_calibration_SW_1", new GeoPosition(x2, 0.5, y2)); // SW
        initialObjects.put("test_calibration_W__1", new GeoPosition(x2, 0.5, y1)); // W
        initialObjects.put("test_calibration_NW_1", new GeoPosition(x2, 0.5, y0)); // NW
        initialObjects.put("test_calibration_100M", new GeoPosition(15.5540, 0.0, y1)); // NW

        gameObjectRepository.deleteAll();

        for (Map.Entry<String, GeoPosition> entry : initialObjects.entrySet()) {

            entry.getValue().setLongitude(
                    entry.getValue().getLongitude()
                            + elevationService.getAltitude(entry.getValue()));

            GameObject gameObject = new GameObject();
            gameObject.setGeoPosition(entry.getValue());
            gameObject.setName(entry.getKey());
            gameObjectRepository.save(gameObject);
        }
    }

}
