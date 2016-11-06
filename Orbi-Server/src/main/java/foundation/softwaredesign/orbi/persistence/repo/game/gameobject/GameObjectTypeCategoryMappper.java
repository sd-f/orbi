package foundation.softwaredesign.orbi.persistence.repo.game.gameobject;

import foundation.softwaredesign.orbi.model.game.gameobject.GameObjectTypeCategory;
import foundation.softwaredesign.orbi.persistence.entity.GameObjectTypeCategoryEntity;
import org.apache.deltaspike.data.api.mapping.SimpleQueryInOutMapperBase;

import static java.util.Objects.isNull;
import static java.util.Objects.nonNull;

/**
 * @author Lucas Reeh <lr86gm@gmail.com>
 */
public class GameObjectTypeCategoryMappper extends SimpleQueryInOutMapperBase<GameObjectTypeCategoryEntity, GameObjectTypeCategory> {

    @Override
    protected Object getPrimaryKey(GameObjectTypeCategory dto) {
        return dto.getId();
    }

    @Override
    protected GameObjectTypeCategory toDto(GameObjectTypeCategoryEntity entity) {
        GameObjectTypeCategory dto = new GameObjectTypeCategory();
        dto.setId(entity.getId());
        dto.setName(entity.getName());
        dto.setCraftable(entity.getCraftable());
        if (nonNull(entity.getGameObjectTypeEntities())) {
            entity.getGameObjectTypeEntities()
                    .forEach(gameObjectTypeEntity -> dto.getTypes().add(new GameObjectTypeMappper().toSimpleDto(gameObjectTypeEntity)));
        }
        return dto;
    }

    @Override
    protected GameObjectTypeCategoryEntity toEntity(GameObjectTypeCategoryEntity oldEntity, GameObjectTypeCategory dto) {
        GameObjectTypeCategoryEntity newEntity = oldEntity;
        if (isNull(oldEntity)) {
            newEntity = new GameObjectTypeCategoryEntity();
        }
        newEntity.setCraftable(dto.getCraftable());
        newEntity.setName(dto.getName());
        return newEntity;
    }
}
