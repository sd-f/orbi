package foundation.softwaredesign.orbi.persistence.repo.game.gameobject;

import foundation.softwaredesign.orbi.persistence.entity.GameObjectTypeEntity;
import org.apache.deltaspike.data.api.AbstractEntityRepository;
import org.apache.deltaspike.data.api.Query;
import org.apache.deltaspike.data.api.Repository;

import javax.validation.constraints.NotNull;
import java.util.List;

/**
 * @author Lucas Reeh <lr86gm@gmail.com>
 */
@Repository
public abstract class GameObjectTypeRepository extends AbstractEntityRepository<GameObjectTypeEntity, Long> {

    public abstract GameObjectTypeEntity findByPrefab(@NotNull String prefab);

    @Query(" select e" +
            "  from GameObjectTypeEntity e" +
            "     INNER JOIN e.gameObjectTypeCategoryEntity c" +
            " where c.craftable = ?1")
    public abstract List<GameObjectTypeEntity> findAllCraftable(Boolean craftable);
}
