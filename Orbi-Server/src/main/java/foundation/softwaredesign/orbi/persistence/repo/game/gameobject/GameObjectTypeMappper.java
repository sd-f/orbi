package foundation.softwaredesign.orbi.persistence.repo.game.gameobject;

import foundation.softwaredesign.orbi.model.game.gameobject.GameObjectType;
import foundation.softwaredesign.orbi.persistence.entity.GameObjectTypeEntity;
import org.apache.deltaspike.data.api.mapping.SimpleQueryInOutMapperBase;

import static java.util.Objects.isNull;
import static java.util.Objects.nonNull;

/**
 * @author Lucas Reeh <lr86gm@gmail.com>
 */
public class GameObjectTypeMappper extends SimpleQueryInOutMapperBase<GameObjectTypeEntity, GameObjectType> {

    @Override
    protected Object getPrimaryKey(GameObjectType dto) {
        return dto.getId();
    }

    @Override
    public GameObjectType toDto(GameObjectTypeEntity entity) {
        GameObjectType dto = new GameObjectType();
        dto.setId(entity.getId());
        dto.setPrefab(entity.getPrefab());
        dto.setRarity(entity.getRarity());
        dto.setCategoryId(entity.getCategoryId());
        dto.setOrdering(entity.getOrdering());
        dto.setSpawnAmount(entity.getSpawnAmount());
        dto.setSupportsUserText(entity.getSupportsUserText());
        dto.setGift(entity.getGift());
        dto.setAi(entity.getAi());
        if (nonNull(entity.getGameObjectTypeCategoryEntity()))
            dto.setCategory(new GameObjectTypeCategoryMappper().toDto(entity.getGameObjectTypeCategoryEntity()));
        return dto;
    }

    @Override
    public GameObjectTypeEntity toEntity(GameObjectTypeEntity oldEntity, GameObjectType dto) {
        GameObjectTypeEntity newEntity = oldEntity;
        if (isNull(oldEntity)) {
            newEntity = new GameObjectTypeEntity();
        }
        newEntity.setId(dto.getId());
        newEntity.setPrefab(dto.getPrefab());
        newEntity.setRarity(dto.getRarity());
        newEntity.setCategoryId(dto.getCategoryId());
        newEntity.setSupportsUserText(dto.getSupportsUserText());
        newEntity.setSpawnAmount(dto.getSpawnAmount());
        newEntity.setOrdering(dto.getOrdering());
        newEntity.setGift(dto.getGift());
        newEntity.setAi(dto.getAi());
        return newEntity;
    }
}
