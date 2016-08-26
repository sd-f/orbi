package foundation.softwaredesign.orbi.persistence.repo;

import foundation.softwaredesign.orbi.model.GameObject;
import foundation.softwaredesign.orbi.model.GeoPosition;
import foundation.softwaredesign.orbi.model.Rotation;
import foundation.softwaredesign.orbi.persistence.entity.GameObjectEntity;
import org.apache.deltaspike.data.api.mapping.SimpleQueryInOutMapperBase;

import static java.util.Objects.isNull;
import static java.util.Objects.nonNull;

/**
 * @author Lucas Reeh <lr86gm@gmail.com>
 */
public class GameObjectMappper extends SimpleQueryInOutMapperBase<GameObjectEntity, GameObject> {

    @Override
    protected Object getPrimaryKey(GameObject gameObject) {
        return gameObject.getId();
    }

    @Override
    protected GameObject toDto(GameObjectEntity cubeEntity) {
        GameObject gameObject = new GameObject();
        gameObject.setId(cubeEntity.getId());
        gameObject.setGeoPosition(new GeoPosition());
        gameObject.getGeoPosition().setLatitude(cubeEntity.getLatitude());
        gameObject.getGeoPosition().setLongitude(cubeEntity.getLongitude());
        gameObject.getGeoPosition().setAltitude(cubeEntity.getAltitude());
        gameObject.setRotation(new Rotation());
        gameObject.getRotation().setY(cubeEntity.getRotationY());
        gameObject.setPrefab(cubeEntity.getPrefab());

        gameObject.setName(cubeEntity.getName());
        return gameObject;
    }

    @Override
    protected GameObjectEntity toEntity(GameObjectEntity gameObjectEntity, GameObject gameObject) {
        GameObjectEntity newGameObjectEntity = gameObjectEntity;
        if (isNull(newGameObjectEntity)) {
            newGameObjectEntity = new GameObjectEntity();
        }
        if (nonNull(gameObject.getGeoPosition())) {
            newGameObjectEntity.setLatitude(gameObject.getGeoPosition().getLatitude());
            newGameObjectEntity.setLongitude(gameObject.getGeoPosition().getLongitude());
            newGameObjectEntity.setAltitude(gameObject.getGeoPosition().getAltitude());
        }
        if (nonNull(gameObject.getRotation())) {
            newGameObjectEntity.setRotationY(gameObject.getRotation().getY());
        }
        newGameObjectEntity.setPrefab(gameObject.getPrefab());
        newGameObjectEntity.setUserId(new Long(1));
        newGameObjectEntity.setName(gameObject.getName());
        return newGameObjectEntity;
    }
}
