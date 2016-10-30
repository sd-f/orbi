package foundation.softwaredesign.orbi.persistence.repo.game.gameobject;

import foundation.softwaredesign.orbi.model.game.gameobject.GameObjectType;
import foundation.softwaredesign.orbi.persistence.entity.GameObjectTypeEntity;
import org.apache.deltaspike.data.api.AbstractEntityRepository;
import org.apache.deltaspike.data.api.Query;
import org.apache.deltaspike.data.api.Repository;
import org.apache.deltaspike.data.api.mapping.MappingConfig;

import javax.validation.constraints.NotNull;
import java.util.List;

/**
 * @author Lucas Reeh <lr86gm@gmail.com>
 */
@Repository(forEntity = GameObjectTypeEntity.class)
@MappingConfig(GameObjectTypeMappper.class)
public abstract class GameObjectTypeRepository extends AbstractEntityRepository<GameObjectType, Long> {

    public abstract GameObjectType findByPrefab(@NotNull String prefab);

    @Query(" select e" +
            "  from GameObjectTypeEntity e" +
            "     INNER JOIN e.gameObjectTypeCategoryEntity c" +
            " where c.craftable = ?1" +
            " order by c.ordering")
    public abstract List<GameObjectType> findAllCraftable(Boolean craftable);

    public abstract List<GameObjectType> findAllOrderByOrdering();
}
