package foundation.softwaredesign.orbi.service.game.character;

import foundation.softwaredesign.orbi.model.game.character.Character;
import foundation.softwaredesign.orbi.model.game.transform.GeoPosition;
import foundation.softwaredesign.orbi.model.game.transform.Position;
import foundation.softwaredesign.orbi.model.game.transform.Transform;
import foundation.softwaredesign.orbi.persistence.repo.game.character.CharacterRepository;
import foundation.softwaredesign.orbi.persistence.repo.game.character.CharacterStatisticsRepository;
import foundation.softwaredesign.orbi.service.auth.UserService;
import foundation.softwaredesign.orbi.service.game.world.WorldAdapterService;
import org.apache.commons.lang3.RandomStringUtils;

import javax.enterprise.context.RequestScoped;
import javax.inject.Inject;
import javax.persistence.NoResultException;
import javax.transaction.Transactional;
import java.util.Calendar;
import java.util.Date;
import java.util.List;
import java.util.Objects;
import java.util.logging.Logger;

import static java.util.Objects.isNull;
import static java.util.Objects.nonNull;

/**
 * @author Lucas Reeh <lr86gm@gmail.com>
 */
@RequestScoped
public class CharacterService {

    @Inject
    CharacterRepository repository;
    @Inject
    CharacterStatisticsRepository statisticsRepository;
    @Inject
    UserService user;
    @Inject
    WorldAdapterService worldAdapter;

    public Character loadByIdentityId(Long identityId) {
        return repository.findByIdentityId(identityId);
    }

    public Character loadById(Long id) {
        return repository.findBy(id);
    }

    public Character loadCurrent() {
        Long identityId = user.getIdentity().getId();
        Character character = loadByIdentityId(identityId);

        return character;
    }

    public void calculateExperienceRank(Character character) {
        Long maxXp = statisticsRepository.findMaxXp();
        Long xr = new Long(0);
        if (nonNull(maxXp) && !maxXp.equals(new Long(0)) && !character.getXp().equals(new Long(0))) {
            Double xp = character.getXp().doubleValue();
            Double maxXPDouble = maxXp.doubleValue();
            Double percentage = xp / maxXPDouble * 100d;
            xr = percentage.longValue();
        }
        character.setXr(xr);
    }

    public Character incrementXp(Long by) {
        Character character = loadCurrent();
        character.setXp(character.getXp() + by);
        return repository.saveAndFlushAndRefresh(character);
    }

    public Character create(Long identityId) {
        Character currentCharacter = new Character();
        currentCharacter.setLastSeen(new Date());
        currentCharacter.setTransform(new Transform());
        currentCharacter.setXp(new Long(0));
        currentCharacter.setIdentityId(identityId);
        currentCharacter.setName(RandomStringUtils.randomAlphanumeric(10).toUpperCase());
        return currentCharacter;
    }

    public Character save(Character character) {
        return repository.saveAndFlushAndRefresh(character);
    }

    public Character updateTransform(Transform newTransform) {
        Character currentCharacter = loadCurrent();
        currentCharacter.setTransform(newTransform);
        currentCharacter.setLastSeen(new Date());
        return save(currentCharacter);
    }

    public List<Character> getCharactersAround(GeoPosition geoPosition) {
        Character mySelf = loadCurrent();
        GeoPosition north = worldAdapter.toGeo(new Position(0d,0d,128d), geoPosition);
        GeoPosition south = worldAdapter.toGeo(new Position(0d,0d,-128d), geoPosition);
        GeoPosition west = worldAdapter.toGeo(new Position(-128d,0d,0d), geoPosition);
        GeoPosition east = worldAdapter.toGeo(new Position(128d,0d,0d), geoPosition);
        List<Character> characterList = repository.findCharactersAround(north.getLatitude(),south.getLatitude(),west.getLongitude(),east.getLongitude());
        Calendar cal = Calendar.getInstance();
        cal.add(Calendar.MINUTE, -1);
        characterList.removeIf(character -> (character.getId().equals(mySelf.getId()) || character.getLastSeen().before(cal.getTime())));
        return characterList;
    }

    public Long count() {
        return statisticsRepository.countAllCharacters();
    }

    public void calculateLevel(Character currentCharacter) {
        currentCharacter.setLevel(new Long(0));
        if (nonNull(currentCharacter.getXp()) && (currentCharacter.getXp().longValue() != 0)) {
            long level = (long)((long)Math.log(currentCharacter.getXp()) / Math.log(16));
            currentCharacter.setLevel(new Long(level + 1l));
        }

    }

    public void gifted() {
        Character currentCharacter = loadCurrent();
        currentCharacter.setGiftedOn(new Date());
        repository.saveAndFlushAndRefresh(currentCharacter);
    }
}
