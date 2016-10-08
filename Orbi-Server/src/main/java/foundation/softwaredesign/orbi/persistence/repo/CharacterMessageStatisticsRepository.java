package foundation.softwaredesign.orbi.persistence.repo;

import foundation.softwaredesign.orbi.persistence.entity.CharacterMessageEntity;
import org.apache.deltaspike.data.api.EntityRepository;
import org.apache.deltaspike.data.api.Query;
import org.apache.deltaspike.data.api.Repository;

/**
 * @author Lucas Reeh <lr86gm@gmail.com>
 */
@Repository
public interface CharacterMessageStatisticsRepository extends EntityRepository<CharacterMessageEntity, Long> {

    @Query(value = "" +
            "select count(e)" +
            "  from CharacterMessageEntity e" +
            " where e.toCharacterId = ?1")
    Long countByToCharacterId(Long characterId);

}
