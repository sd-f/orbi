package foundation.softwaredesign.orbi.persistence.repo;

import foundation.softwaredesign.orbi.model.GameObject;
import foundation.softwaredesign.orbi.persistence.entity.GameObjectEntity;
import org.apache.deltaspike.data.api.EntityRepository;
import org.apache.deltaspike.data.api.Modifying;
import org.apache.deltaspike.data.api.Query;
import org.apache.deltaspike.data.api.Repository;
import org.apache.deltaspike.data.api.mapping.MappingConfig;

import java.util.List;

/**
 * @author Lucas Reeh <lr86gm@gmail.com>
 */
@Repository(forEntity = GameObjectEntity.class)
@MappingConfig(GameObjectMappper.class)
public interface GameObjectRepository extends EntityRepository<GameObject, Long> {

    @Modifying
    @Query("delete from GameObjectEntity")
    void deleteAll();

    @Query(" select e" +
            "  from GameObjectEntity e" +
            " where ( e.longitude between (?2 - 0.002) and (?2 + 0.002))" +
            "   and ( e.latitude between (?1 - 0.002) and (?1 + 0.002))")
    List<GameObject> findGameObjectsAround(Double latitude, Double longitude);
}
