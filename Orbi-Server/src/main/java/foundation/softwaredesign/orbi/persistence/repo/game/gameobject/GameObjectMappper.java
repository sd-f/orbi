package foundation.softwaredesign.orbi.persistence.repo.game.gameobject;

import foundation.softwaredesign.orbi.model.game.gameobject.GameObject;
import foundation.softwaredesign.orbi.persistence.entity.GameObjectEntity;
import foundation.softwaredesign.orbi.service.game.gameobject.GameObjectTypeService;
import foundation.softwaredesign.orbi.service.auth.UserService;
import org.apache.deltaspike.data.api.mapping.SimpleQueryInOutMapperBase;

import javax.inject.Inject;
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
        gameObject.getTransform().getGeoPosition().setLatitude(objectEntity.getLatitude());
        gameObject.getTransform().getGeoPosition().setLongitude(objectEntity.getLongitude());
        gameObject.getTransform().getGeoPosition().setAltitude(objectEntity.getAltitude());
        gameObject.getTransform().getRotation().setY(objectEntity.getRotationY());
        if (nonNull(objectEntity.getGameObjectType())) {
            gameObject.setType(new GameObjectTypeMappper().toDto(objectEntity.getGameObjectType()));
        }
        gameObject.setCreateDate(objectEntity.getCreateDate());
        gameObject.setIdentityId(objectEntity.getIdentity().getId());
        gameObject.setUserText(objectEntity.getUserText());
        gameObject.setName(objectEntity.getName());
        gameObject.setConstraints(objectEntity.getBodyConstraints());
        return gameObject;
    }

    @Override
    protected GameObjectEntity toEntity(GameObjectEntity gameObjectEntity, GameObject gameObject) {
        GameObjectEntity newGameObjectEntity = gameObjectEntity;
        if (isNull(gameObjectEntity)) {
            newGameObjectEntity = new GameObjectEntity();
        }
        if (isNull(newGameObjectEntity.getId())) {
            newGameObjectEntity.setIdentity(userService.getIdentity());
            newGameObjectEntity.setCreateDate(new Date());
        }
        if (nonNull(gameObject.getTransform())) {
            if (nonNull(gameObject.getTransform().getGeoPosition())) {
                newGameObjectEntity.setLatitude(gameObject.getTransform().getGeoPosition().getLatitude());
                newGameObjectEntity.setLongitude(gameObject.getTransform().getGeoPosition().getLongitude());
                newGameObjectEntity.setAltitude(gameObject.getTransform().getGeoPosition().getAltitude());
            }
            if (nonNull(gameObject.getTransform().getRotation())) {
                newGameObjectEntity.setRotationY(gameObject.getTransform().getRotation().getY());
            }
        }
        newGameObjectEntity.setUserText(gameObject.getUserText());
        newGameObjectEntity.setGameObjectType(new GameObjectTypeMappper().toEntity(null, gameObjectType.loadByPrefab(gameObject.getType().getPrefab())));

        newGameObjectEntity.setName(gameObject.getName());
        newGameObjectEntity.setBodyConstraints(gameObject.getConstraints());
        return newGameObjectEntity;
    }
}
