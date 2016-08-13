package foundation.softwaredesign.orbi.persistence.repo;

import foundation.softwaredesign.orbi.model.virtual.GameObject;
import foundation.softwaredesign.orbi.model.virtual.Position;
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
        gameObject.setPosition(new Position());
        gameObject.getPosition().setX(cubeEntity.getLongitude());
        gameObject.getPosition().setY(cubeEntity.getElevation());
        gameObject.getPosition().setZ(cubeEntity.getLatitude());
        return gameObject;
    }

    @Override
    protected GameObjectEntity toEntity(GameObjectEntity gameObjectEntity, GameObject gameObject) {
        GameObjectEntity newGameObject = gameObjectEntity;
        if (isNull(newGameObject)) {
            newGameObject = new GameObjectEntity();
        }
        if (nonNull(gameObject.getPosition())) {
            newGameObject.setLongitude(gameObject.getPosition().getX());
            newGameObject.setElevation(gameObject.getPosition().getY());
            newGameObject.setLatitude(gameObject.getPosition().getZ());
        }
        return newGameObject;
    }
}
