package foundation.softwaredesign.orbi.service.game.world;

import foundation.softwaredesign.orbi.model.game.gameobject.GameObject;
import foundation.softwaredesign.orbi.model.game.transform.GeoPosition;
import foundation.softwaredesign.orbi.model.game.transform.Rotation;
import foundation.softwaredesign.orbi.service.auth.UserService;
import foundation.softwaredesign.orbi.service.game.gameobject.GameObjectService;

import javax.enterprise.context.RequestScoped;
import javax.inject.Inject;
import javax.ws.rs.ForbiddenException;
import java.util.Date;
import java.util.HashMap;
import java.util.Map;

/**
 * @author Lucas Reeh <lr86gm@gmail.com>
 */
@RequestScoped
public class WorldFactory {

    @Inject
    GameObjectService gameObjectService;
    @Inject
    ElevationService elevationService;
    @Inject
    UserService userService;

    Map<String, GeoPosition> initialObjects;

    public void reset() {
        if (userService.getIdentity().getId() != 0) {
            throw new ForbiddenException("You need more rights to do that");
        }

        initialObjects = new HashMap<>();
        Double right = 15.5554; // E
        Double left = 15.5550; // W

        Double top = 47.0680; // N
        Double bottom = 47.0676; // S

        Double middle = 47.0678;
        Double center = 15.5552;

        initialObjects.put("test_calibration_N__1", new GeoPosition(top, center, 0.0000001)); // N
        initialObjects.put("test_calibration_N__2", new GeoPosition(top, center, 1.0000002)); // N
        initialObjects.put("test_calibration_N__3", new GeoPosition(top, center, 2.0000003)); // N
        initialObjects.put("test_calibration_N__4", new GeoPosition(top, center, 3.0000004)); // N
        initialObjects.put("test_calibration_NE_1", new GeoPosition(top, right, 0.0000001)); // NE
        initialObjects.put("test_calibration_E__1", new GeoPosition(middle, right, 0.0000001)); // E
        initialObjects.put("test_calibration_E__2", new GeoPosition(middle, right, 1.0000002)); // E
        initialObjects.put("test_calibration_E__3", new GeoPosition(middle, right, 2.0000003)); // E
        initialObjects.put("test_calibration_SE_1", new GeoPosition(bottom, right, 0.0000001)); // SE
        initialObjects.put("test_calibration_S__1", new GeoPosition(bottom, center, 0.0000001)); // S
        initialObjects.put("test_calibration_S__2", new GeoPosition(bottom, center, 1.0000002)); // S
        initialObjects.put("test_calibration_SW_1", new GeoPosition(bottom, left, 0.0000001)); // SW
        initialObjects.put("test_calibration_W__1", new GeoPosition(middle, left, 0.0000001)); // W
        initialObjects.put("test_calibration_NW_1", new GeoPosition(top, left, 0.0000001)); // NW
        initialObjects.put("test_calibration_100M", new GeoPosition(middle, 15.5540, 0.0000001)); // NW

        gameObjectService.deleteAll();

        for (Map.Entry<String, GeoPosition> entry : initialObjects.entrySet()) {
            GameObject gameObject = new GameObject();
            gameObject.getTransform().setGeoPosition(entry.getValue());
            gameObject.setName(entry.getKey());
            gameObject.setCreateDate(new Date());
            gameObject.setIdentityId(userService.getIdentity().getId());
            gameObject.getTransform().setRotation(new Rotation(0d,0d,0d));
            gameObject.setPrefab("ScifiCrate/ScifiCrate_1");
            if (gameObject.getName().contains("1")) {
                gameObject.setPrefab("ScifiCrate/ScifiCrate_2");
            }
            gameObjectService.save(gameObject);
        }
    }

}
