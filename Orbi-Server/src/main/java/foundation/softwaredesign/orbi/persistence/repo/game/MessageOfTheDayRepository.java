package foundation.softwaredesign.orbi.persistence.repo.game;

import foundation.softwaredesign.orbi.model.game.server.MessageOfTheDay;
import foundation.softwaredesign.orbi.persistence.entity.MessageOfTheDayEntity;
import org.apache.deltaspike.data.api.EntityRepository;
import org.apache.deltaspike.data.api.Query;
import org.apache.deltaspike.data.api.Repository;
import org.apache.deltaspike.data.api.mapping.MappingConfig;

import java.util.Date;
import java.util.List;

/**
 * @author Lucas Reeh <lr86gm@gmail.com>
 */
@Repository(forEntity = MessageOfTheDayEntity.class)
@MappingConfig(MessageOfTheDayMappper.class)
public interface MessageOfTheDayRepository extends EntityRepository<MessageOfTheDay, Long> {

    @Query(" select e" +
            "  from MessageOfTheDayEntity e" +
            " where e.expires is null" +
            "    or e.expires > ?1")
    List<MessageOfTheDay> findAllValid(Date before);

}
