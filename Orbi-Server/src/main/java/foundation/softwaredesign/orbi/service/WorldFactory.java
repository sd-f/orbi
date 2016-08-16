package foundation.softwaredesign.orbi.service;

import foundation.softwaredesign.orbi.model.virtual.GameObject;
import foundation.softwaredesign.orbi.model.virtual.Position;
import foundation.softwaredesign.orbi.model.virtual.World;
import foundation.softwaredesign.orbi.persistence.repo.ElevationRepository;
import foundation.softwaredesign.orbi.persistence.repo.GameObjectRepository;
import javafx.geometry.Pos;

import javax.enterprise.context.RequestScoped;
import javax.inject.Inject;
import java.math.BigDecimal;
import java.math.BigInteger;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

import static java.util.Objects.nonNull;

/**
 * @author Lucas Reeh <lr86gm@gmail.com>
 */
@RequestScoped
public class WorldFactory {

    @Inject
    GameObjectRepository gameObjectRepository;

    @Inject
    ElevationRepository elevationRepository;

    Map<String, Position> initialObjects;

    public void init() {
        initialObjects = new HashMap<>();
        initialObjects.put("test_calibration_N_1", new Position(new BigDecimal(47.0677), new BigDecimal(0), new BigDecimal(15.5552)));
        initialObjects.put("test_calibration_N_2", new Position(new BigDecimal(47.0677), new BigDecimal(1.001),new BigDecimal(15.5552) )); // N
        initialObjects.put("test_calibration_N_3", new Position(new BigDecimal(47.0677), new BigDecimal(2.002), new BigDecimal(15.5552))); // N
        initialObjects.put("test_calibration_N_4", new Position(new BigDecimal(47.0677), new BigDecimal(3.003),new BigDecimal(15.5552) )); // N
        initialObjects.put("test_calibration_NE_1", new Position(new BigDecimal(47.0677), new BigDecimal(0), new BigDecimal(15.5554))); // NE
        initialObjects.put("test_calibration_E_1", new Position(new BigDecimal(47.0676), new BigDecimal(0), new BigDecimal(15.5554))); // E
        initialObjects.put("test_calibration_E_2", new Position(new BigDecimal(47.0676), new BigDecimal(1.001), new BigDecimal(15.5554))); // E
        initialObjects.put("test_calibration_E_3", new Position(new BigDecimal(47.0676), new BigDecimal(2.002), new BigDecimal(15.5554))); // E
        initialObjects.put("test_calibration_SE_1", new Position(new BigDecimal(47.0675), new BigDecimal(0), new BigDecimal(15.5554))); // SE
        initialObjects.put("test_calibration_S_1", new Position(new BigDecimal(47.0675), new BigDecimal(0), new BigDecimal(15.5552))); // S
        initialObjects.put("test_calibration_S_2", new Position(new BigDecimal(47.0675), new BigDecimal(1.001),new BigDecimal(15.5552) )); // S
        initialObjects.put("test_calibration_SW", new Position(new BigDecimal(47.0675), new BigDecimal(0), new BigDecimal(15.5550))); // SW
        initialObjects.put("test_calibration_W", new Position(new BigDecimal(47.0676), new BigDecimal(0), new BigDecimal(15.5550))); // W
        initialObjects.put("test_calibration_NW", new Position(new BigDecimal(47.0677), new BigDecimal(0), new BigDecimal(15.5550))); // NW

        gameObjectRepository.deleteAll();

        for (Map.Entry<String, Position> entry : initialObjects.entrySet()) {
            entry.getValue().setY(
                    entry.getValue().getY().add(
                            new BigDecimal(
                                    elevationRepository
                                            .getElevation(entry.getValue().getX(), entry.getValue().getZ()))));
            GameObject gameObject = new GameObject();
            gameObject.setPosition(entry.getValue());
            gameObject.setName(entry.getKey());
            gameObjectRepository.save(gameObject);
        }
    }

    public World generateRasterAroundPosition(final Position centerPosition, final Integer rasterInterval) {
        World terrain = new World();
        List<GameObject> dummyGameObjects = new ArrayList<>();

        Double scale = 0.0001;

        Position startPosition = new Position(
                centerPosition.getX().subtract(BigDecimal.valueOf(scale * (rasterInterval/2f))),
                centerPosition.getY(),
                centerPosition.getZ().subtract(BigDecimal.valueOf(scale * (rasterInterval/2f))));

        for (Integer x = 0; x < rasterInterval; x++) {
            for (Integer z = 0; z < rasterInterval; z++) {
                Position newPosition = new Position(
                        startPosition.getX().add(new BigDecimal((x+1)*scale)),
                        startPosition.getY(),
                        startPosition.getZ().add(new BigDecimal((z+1)*scale))
                );
                GameObject dummyGameObject = new GameObject();
                BigInteger id = BigInteger.valueOf((x+1)*(z+1));
                dummyGameObject.setId(id);
                dummyGameObject.setPosition(newPosition);
                dummyGameObjects.add(dummyGameObject);
            }
        }
        terrain.setGameObjects(dummyGameObjects);
        return terrain;
    }

    public void addElevations(World world) {
        for (GameObject gameObject: world.getGameObjects()) {
            addElevation(gameObject.getPosition());
        }
    }

    public void addElevation(Position position) {
        Double elevation = elevationRepository.getElevation(position.getX(), position.getZ());
        if (nonNull(elevation)) {
            position.setY(new BigDecimal(elevation));
        }
    }
}
