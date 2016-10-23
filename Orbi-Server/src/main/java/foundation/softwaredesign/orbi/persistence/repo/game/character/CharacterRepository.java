package foundation.softwaredesign.orbi.persistence.repo.game.character;

import foundation.softwaredesign.orbi.model.game.character.Character;
import foundation.softwaredesign.orbi.persistence.entity.CharacterEntity;
import org.apache.deltaspike.data.api.EntityRepository;
import org.apache.deltaspike.data.api.Query;
import org.apache.deltaspike.data.api.Repository;
import org.apache.deltaspike.data.api.mapping.MappingConfig;

import java.util.List;

/**
 * @author Lucas Reeh <lr86gm@gmail.com>
 */
@Repository(forEntity = CharacterEntity.class)
@MappingConfig(CharacterMappper.class)
public interface CharacterRepository extends EntityRepository<Character, Long> {

    @Query(" select e" +
            "  from CharacterEntity e" +
            " where ( e.longitude between (?3) and (?4))" +
            "   and ( e.latitude between (?2) and (?1))")
    List<Character> findCharactersAround(Double north, Double south, Double west, Double east);

    Character findByIdentityId(Long identityId);
}
