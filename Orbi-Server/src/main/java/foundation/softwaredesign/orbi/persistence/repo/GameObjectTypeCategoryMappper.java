package foundation.softwaredesign.orbi.persistence.repo;

import foundation.softwaredesign.orbi.model.CharacterMessage;
import foundation.softwaredesign.orbi.model.GameObjectTypeCategory;
import foundation.softwaredesign.orbi.persistence.entity.CharacterMessageEntity;
import foundation.softwaredesign.orbi.persistence.entity.GameObjectTypeCategoryEntity;
import org.apache.deltaspike.data.api.mapping.SimpleQueryInOutMapperBase;

import java.util.Date;

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
        dto.setRarity(entity.getRarity());
        dto.setCraftable(entity.getCraftable());
        dto.setNumberOfItems(new Long(0));
        if (nonNull(entity.getGameObjectTypeEntities())) {
            dto.setNumberOfItems(new Long(entity.getGameObjectTypeEntities().size()));
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
        newEntity.setRarity(dto.getRarity());
        return newEntity;
    }
}
