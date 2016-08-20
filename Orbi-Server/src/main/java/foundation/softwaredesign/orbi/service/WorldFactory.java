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
        Double right = 15.5554; // E
        Double left = 15.5550; // W

        Double top = 47.0680; // N
        Double bottom = 47.0676; // S

        Double middle = 47.0678;
        Double center = 15.5552;

        initialObjects.put("test_calibration_N__1", new GeoPosition(top, center, 0.5)); // N
        initialObjects.put("test_calibration_N__2", new GeoPosition(top, center, 1.5)); // N
        initialObjects.put("test_calibration_N__3", new GeoPosition(top, center, 2.5)); // N
        initialObjects.put("test_calibration_N__4", new GeoPosition(top, center, 3.5)); // N
        initialObjects.put("test_calibration_NE_1", new GeoPosition(top, right, 0.5)); // NE
        initialObjects.put("test_calibration_E__1", new GeoPosition(middle, right, 0.5)); // E
        initialObjects.put("test_calibration_E__2", new GeoPosition(middle, right, 1.5)); // E
        initialObjects.put("test_calibration_E__3", new GeoPosition(middle, right, 2.5)); // E
        initialObjects.put("test_calibration_SE_1", new GeoPosition(bottom, right, 0.5)); // SE
        initialObjects.put("test_calibration_S__1", new GeoPosition(bottom, center, 0.5)); // S
        initialObjects.put("test_calibration_S__2", new GeoPosition(bottom, center, 1.5)); // S
        initialObjects.put("test_calibration_SW_1", new GeoPosition(bottom, left, 0.5)); // SW
        initialObjects.put("test_calibration_W__1", new GeoPosition(bottom, left, 0.5)); // W
        initialObjects.put("test_calibration_NW_1", new GeoPosition(top, left, 0.5)); // NW
        initialObjects.put("test_calibration_100M", new GeoPosition(middle, 15.5540, 0.0)); // NW

        gameObjectRepository.deleteAll();

        for (Map.Entry<String, GeoPosition> entry : initialObjects.entrySet()) {

            entry.getValue().setAltitude(
                    entry.getValue().getLongitude()
                            + elevationService.getAltitude(entry.getValue()));

            GameObject gameObject = new GameObject();
            gameObject.setGeoPosition(entry.getValue());
            gameObject.setName(entry.getKey());
            gameObjectRepository.save(gameObject);
        }
    }

}
