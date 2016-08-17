package foundation.softwaredesign.orbi.service;

import foundation.softwaredesign.orbi.model.virtual.GameObject;
import foundation.softwaredesign.orbi.model.virtual.Position;
import foundation.softwaredesign.orbi.model.virtual.World;
import foundation.softwaredesign.orbi.persistence.repo.ElevationRepository;
import foundation.softwaredesign.orbi.persistence.repo.GameObjectRepository;
import javafx.geometry.Pos;

import javax.enterprise.context.RequestScoped;
import javax.inject.Inject;
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

    @Inject
    VirtualWorldAdapter virtualWorldAdapter;

    @Inject
    RealWorldAdapter realWorldAdapter;

    Map<String, Position> initialObjects;

    public void init() {
        initialObjects = new HashMap<>();
        Double y0 = 47.0680; // N
        Double y2 = 47.0676; // S

        Double x0 = 15.5554; // E
        Double x2 = 15.5550; // W

        Double x1 = 15.5552;
        Double y1 = 47.0678;

        initialObjects.put("test_calibration_N__1", new Position(x1, 0.5, y0)); // N
        initialObjects.put("test_calibration_N__2", new Position(x1, 1.5, y0)); // N
        initialObjects.put("test_calibration_N__3", new Position(x1, 2.5, y0)); // N
        initialObjects.put("test_calibration_N__4", new Position(x1, 3.5, y0)); // N
        initialObjects.put("test_calibration_NE_1", new Position(x0, 0.5, y0)); // NE
        initialObjects.put("test_calibration_E__1", new Position(x0, 0.5, y1)); // E
        initialObjects.put("test_calibration_E__2", new Position(x0, 1.5, y1)); // E
        initialObjects.put("test_calibration_E__3", new Position(x0, 2.5, y1)); // E
        initialObjects.put("test_calibration_SE_1", new Position(x0, 0.5, y2)); // SE
        initialObjects.put("test_calibration_S__1", new Position(x1, 0.5, y2)); // S
        initialObjects.put("test_calibration_S__2", new Position(x1, 1.5, y2)); // S
        initialObjects.put("test_calibration_SW_1", new Position(x2, 0.5, y2)); // SW
        initialObjects.put("test_calibration_W__1", new Position(x2, 0.5, y1)); // W
        initialObjects.put("test_calibration_NW_1", new Position(x2, 0.5, y0)); // NW
        initialObjects.put("test_calibration_100M", new Position(15.5540, 0.0, y1)); // NW

        gameObjectRepository.deleteAll();

        for (Map.Entry<String, Position> entry : initialObjects.entrySet()) {

            entry.getValue().setY(
                    entry.getValue().getY()
                            + elevationRepository.getElevation(
                            entry.getValue().getZ(), entry.getValue().getX()
                    ));

            GameObject gameObject = new GameObject();
            gameObject.setPosition(entry.getValue());
            gameObject.setName(entry.getKey());
            gameObjectRepository.save(gameObject);
        }
    }

    public World generateRasterAroundPosition(final Position centerPosition, final Integer rasterSize) {
        World terrain = new World();
        List<GameObject> dummyGameObjects = new ArrayList<>();
        Integer internalSize = 17;
        Integer rasterScale = (rasterSize / internalSize) + 1;


        for (Integer x = 0; x < internalSize; x++) {
            for (Integer z = 0; z < internalSize; z++) {
                Long id = new Long((x + 1) * (z + 1));
                Position newPosition = new Position(x.doubleValue() * rasterScale, 0.0, z.doubleValue() * rasterScale);
                GameObject dummyGameObject = new GameObject();
                dummyGameObject.setId(id);
                dummyGameObject.setPosition(newPosition);
                dummyGameObjects.add(dummyGameObject);
            }
        }
        terrain.setGameObjects(dummyGameObjects);
        return terrain;
    }

    public void addElevations(World world, Position center) {
        Integer delta = 0; // TODO performance hack
        Double lastElevation = null;
        Double correctMissingElevation = null;
        for (GameObject gameObject : world.getGameObjects()) {
            Position correctedPosition = new Position(gameObject.getPosition().getX() - 64.0,
                    gameObject.getPosition().getY() ,
                    gameObject.getPosition().getZ()- 64.0);
            gameObject.setPosition(correctedPosition);
            if (delta % 1 == 0) {
                Position realPosition = correctedPosition.clone();
                realWorldAdapter.scalePosition(realPosition);
                GameObject dummyGameObject = new GameObject();
                dummyGameObject.setPosition(realPosition);
                realWorldAdapter.translatePosition(dummyGameObject, center);
                lastElevation = elevationRepository.getElevation(dummyGameObject.getPosition().getZ(), dummyGameObject.getPosition().getX());
            }
            if (nonNull(lastElevation)) {
                correctMissingElevation = lastElevation;
                gameObject.getPosition().setY(lastElevation);
            } else {
                gameObject.getPosition().setY(correctMissingElevation);
            }
            delta++;
            //addElevation(gameObject.getPosition());
        }
    }

    public void addElevation(Position position) {
        Double elevation = elevationRepository.getElevation(position.getZ(), position.getX());
        if (nonNull(elevation)) {
            position.setY(elevation);
        }
    }
}
