package foundation.softwaredesign.orbi.persistence.repo;

import foundation.softwaredesign.orbi.model.GameObject;
import foundation.softwaredesign.orbi.model.GeoPosition;
import foundation.softwaredesign.orbi.model.Rotation;
import foundation.softwaredesign.orbi.persistence.entity.GameObjectEntity;
import foundation.softwaredesign.orbi.service.GameObjectTypeService;
import foundation.softwaredesign.orbi.service.UserService;
import org.apache.deltaspike.data.api.mapping.SimpleQueryInOutMapperBase;

import javax.inject.Inject;
import javax.persistence.NoResultException;
import javax.ws.rs.BadRequestException;

import java.util.Date;

import static java.util.Objects.isNull;
import static java.util.Objects.nonNull;

/**
 * @author Lucas Reeh <lr86gm@gmail.com>
 */
public class GameObjectMappper extends SimpleQueryInOutMapperBase<GameObjectEntity, GameObject> {

    @Inject
    UserService userService;
    @Inject
    GameObjectTypeService gameObjectType;

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
        gameObject.setPrefab(objectEntity.getGameObjectType().getPrefab());
        gameObject.setCreateDate(objectEntity.getCreateDate());
        gameObject.setIdentityId(objectEntity.getIdentity().getId());
        gameObject.setName(objectEntity.getName());
        return gameObject;
    }

    @Override
    protected GameObjectEntity toEntity(GameObjectEntity gameObjectEntity, GameObject gameObject) {
        GameObjectEntity newGameObjectEntity = gameObjectEntity;
        if (isNull(newGameObjectEntity.getId())) {
            newGameObjectEntity.setIdentity(userService.getIdentity());
            newGameObjectEntity.setCreateDate(new Date());
        }

        if (nonNull(gameObject.getGeoPosition())) {
            newGameObjectEntity.setLatitude(gameObject.getGeoPosition().getLatitude());
            newGameObjectEntity.setLongitude(gameObject.getGeoPosition().getLongitude());
            newGameObjectEntity.setAltitude(gameObject.getGeoPosition().getAltitude());
        }
        if (nonNull(gameObject.getRotation())) {
            newGameObjectEntity.setRotationY(gameObject.getRotation().getY());
        }
        newGameObjectEntity.setGameObjectType(gameObjectType.loadByPrefab(gameObject.getPrefab()));

        newGameObjectEntity.setName(gameObject.getName());
        return newGameObjectEntity;
    }
}
