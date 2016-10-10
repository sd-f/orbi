package foundation.softwaredesign.orbi.persistence.repo;

import foundation.softwaredesign.orbi.persistence.entity.GameObjectEntity;
import org.apache.deltaspike.data.api.EntityRepository;
import org.apache.deltaspike.data.api.Query;
import org.apache.deltaspike.data.api.Repository;

/**
 * @author Lucas Reeh <lr86gm@gmail.com>
 */
@Repository
public interface GameObjectStatisticsRepository extends EntityRepository<GameObjectEntity, Long> {

    @Query(" select count(e)" +
            "  from GameObjectEntity e")
    Long countAllObjects();
}
