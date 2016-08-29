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
    protected GameObject toDto(GameObjectEntity objectEntity) {
        GameObject gameObject = new GameObject();
        gameObject.setId(objectEntity.getId());
        gameObject.setGeoPosition(new GeoPosition());
        gameObject.getGeoPosition().setLatitude(objectEntity.getLatitude());
        gameObject.getGeoPosition().setLongitude(objectEntity.getLongitude());
        gameObject.getGeoPosition().setAltitude(objectEntity.getAltitude());
        gameObject.setRotation(new Rotation());
        gameObject.getRotation().setY(objectEntity.getRotationY());
        gameObject.setPrefab(objectEntity.getPrefab());
        gameObject.setCreateDate(objectEntity.getCreateDate());

        gameObject.setName(objectEntity.getName());
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
