package foundation.softwaredesign.orbi.persistence.repo;

import foundation.softwaredesign.orbi.model.virtual.GameObject;
import foundation.softwaredesign.orbi.model.virtual.Position;
import foundation.softwaredesign.orbi.persistence.entity.GameObjectEntity;
import org.apache.deltaspike.data.api.mapping.SimpleQueryInOutMapperBase;

import java.math.BigDecimal;
import java.math.BigInteger;

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
        gameObject.getPosition().setZ(cubeEntity.getLongitude());
        gameObject.getPosition().setY(cubeEntity.getElevation());
        gameObject.getPosition().setX(cubeEntity.getLatitude());
        gameObject.setName(cubeEntity.getName());
        return gameObject;
    }

    @Override
    protected GameObjectEntity toEntity(GameObjectEntity gameObjectEntity, GameObject gameObject) {
        GameObjectEntity newGameObjectEntity = gameObjectEntity;
        if (isNull(newGameObjectEntity)) {
            newGameObjectEntity = new GameObjectEntity();
        }
        if (nonNull(gameObject.getPosition())) {
            newGameObjectEntity.setLongitude(gameObject.getPosition().getZ());
            newGameObjectEntity.setElevation(gameObject.getPosition().getY());
            newGameObjectEntity.setLatitude(gameObject.getPosition().getX());
        }
        newGameObjectEntity.setUserId(BigInteger.valueOf(1));
        newGameObjectEntity.setName(gameObject.getName());
        return newGameObjectEntity;
    }
}
