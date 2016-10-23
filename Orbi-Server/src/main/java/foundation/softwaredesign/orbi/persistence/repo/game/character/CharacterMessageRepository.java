package foundation.softwaredesign.orbi.persistence.repo.game.character;

import foundation.softwaredesign.orbi.model.game.character.CharacterMessage;
import foundation.softwaredesign.orbi.persistence.entity.CharacterMessageEntity;
import org.apache.deltaspike.data.api.EntityRepository;
import org.apache.deltaspike.data.api.Repository;
import org.apache.deltaspike.data.api.mapping.MappingConfig;

import java.util.List;

/**
 * @author Lucas Reeh <lr86gm@gmail.com>
 */
@Repository(forEntity = CharacterMessageEntity.class)
@MappingConfig(CharacterMessageMappper.class)
public interface CharacterMessageRepository extends EntityRepository<CharacterMessage, Long> {

    List<CharacterMessage> findByToCharacterId(Long characterId);

}
