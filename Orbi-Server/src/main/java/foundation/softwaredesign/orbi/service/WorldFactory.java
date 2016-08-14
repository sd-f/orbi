package foundation.softwaredesign.orbi.service;

import foundation.softwaredesign.orbi.model.virtual.GameObject;
import foundation.softwaredesign.orbi.model.virtual.Position;
import foundation.softwaredesign.orbi.persistence.repo.GameObjectRepository;

import javax.enterprise.context.RequestScoped;
import javax.inject.Inject;
import java.math.BigDecimal;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

/**
 * @author Lucas Reeh <lr86gm@gmail.com>
 */
@RequestScoped
public class WorldFactory {

    @Inject
    GameObjectRepository gameObjectRepository;

    Map<String, Position> initialObjects;

    public void init() {
        initialObjects = new HashMap<>();
        initialObjects.put("test_calibration_N_1", new Position(new BigDecimal(15.5552), new BigDecimal(0), new BigDecimal(47.0677)));
        initialObjects.put("test_calibration_N_2", new Position(new BigDecimal(15.5552), new BigDecimal(1.0001), new BigDecimal(47.0677))); // N
        initialObjects.put("test_calibration_N_3", new Position(new BigDecimal(15.5552), new BigDecimal(2.0002), new BigDecimal(47.0677))); // N
        initialObjects.put("test_calibration_N_4", new Position(new BigDecimal(15.5552), new BigDecimal(3.0003), new BigDecimal(47.0677))); // N
        initialObjects.put("test_calibration_NE_1", new Position(new BigDecimal(15.5554), new BigDecimal(0), new BigDecimal(47.0677))); // NE
        initialObjects.put("test_calibration_E_1", new Position(new BigDecimal(15.5554), new BigDecimal(0), new BigDecimal(47.0676))); // E
        initialObjects.put("test_calibration_E_2", new Position(new BigDecimal(15.5554), new BigDecimal(1.0001), new BigDecimal(47.0676))); // E
        initialObjects.put("test_calibration_E_3", new Position(new BigDecimal(15.5554), new BigDecimal(2.0002), new BigDecimal(47.0676))); // E
        initialObjects.put("test_calibration_SE_1", new Position(new BigDecimal(15.5554), new BigDecimal(0), new BigDecimal(47.0675))); // SE
        initialObjects.put("test_calibration_S_1", new Position(new BigDecimal(15.5552), new BigDecimal(0), new BigDecimal(47.0675))); // S
        initialObjects.put("test_calibration_S_2", new Position(new BigDecimal(15.5552), new BigDecimal(1.0001), new BigDecimal(47.0675))); // S
        initialObjects.put("test_calibration_SW", new Position(new BigDecimal(15.5550), new BigDecimal(0), new BigDecimal(47.0675))); // SW
        initialObjects.put("test_calibration_W", new Position(new BigDecimal(15.5550), new BigDecimal(0), new BigDecimal(47.0676))); // W
        initialObjects.put("test_calibration_NW", new Position(new BigDecimal(15.5550), new BigDecimal(0), new BigDecimal(47.0677))); // NW

        gameObjectRepository.deleteAll();

        for (Map.Entry<String, Position> entry : initialObjects.entrySet()) {
            GameObject gameObject = new GameObject();
            gameObject.setPosition(entry.getValue());
            gameObject.setName(entry.getKey());
            gameObjectRepository.save(gameObject);
        }
    }
}
