package foundation.softwaredesign.orbi.persistence.repo.game.gameobject;

import foundation.softwaredesign.orbi.model.game.gameobject.GameObject;
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
            " where ( e.longitude between (?3) and (?4))" +
            "   and ( e.latitude between (?2) and (?1))")
    List<GameObject> findGameObjectsAround(Double north, Double south, Double west, Double east);
}
