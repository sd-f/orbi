package foundation.softwaredesign.orbi.persistence.repo;

import foundation.softwaredesign.orbi.persistence.entity.CharacterEntity;
import org.apache.deltaspike.data.api.EntityRepository;
import org.apache.deltaspike.data.api.Query;
import org.apache.deltaspike.data.api.Repository;

/**
 * @author Lucas Reeh <lr86gm@gmail.com>
 */
@Repository
public interface CharacterStatisticsRepository extends EntityRepository<CharacterEntity, Long> {

    @Query(" select max(e.experiencePoints)" +
            "  from CharacterEntity e")
    Long findMaxXp();

    @Query(" select count(e.experiencePoints)" +
            "  from CharacterEntity e")
    Long countAllCharacters();
}
