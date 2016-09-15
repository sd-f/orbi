package foundation.softwaredesign.orbi.persistence.repo;

import foundation.softwaredesign.orbi.persistence.entity.GameObjectTypeEntity;
import foundation.softwaredesign.orbi.persistence.entity.InventoryEntity;
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
}
