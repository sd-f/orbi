package foundation.softwaredesign.orbi.persistence.repo;

import foundation.softwaredesign.orbi.persistence.entity.InventoryEntity;
import foundation.softwaredesign.orbi.persistence.entity.InventoryEntityId;
import org.apache.deltaspike.data.api.AbstractEntityRepository;
import org.apache.deltaspike.data.api.Query;
import org.apache.deltaspike.data.api.Repository;
import org.apache.deltaspike.data.api.SingleResultType;

import javax.validation.constraints.NotNull;
import java.util.List;

/**
 * @author Lucas Reeh <lr86gm@gmail.com>
 */
@Repository
public abstract class InventoryRepository extends AbstractEntityRepository<InventoryEntity, InventoryEntityId> {

    @Query(value = "" +
            "Select e" +
            "  from InventoryEntity e" +
            " where e.id.identityId = ?1" +
            " order by e.id.gameObjectTypeId")
    public abstract List<InventoryEntity> findByIdentityId(@NotNull Long identityId);

    @Query(value = "" +
            "Select e" +
            "  from InventoryEntity e" +
            " where e.id.identityId = ?1" +
            "   and e.id.gameObjectTypeId = ?2",
            singleResult = SingleResultType.OPTIONAL)
    public abstract InventoryEntity findByIdentAndType(@NotNull Long identityId,
                                                       @NotNull Long gameObjectTypeId);
}
