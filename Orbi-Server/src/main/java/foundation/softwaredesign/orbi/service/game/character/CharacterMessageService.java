package foundation.softwaredesign.orbi.service.game.character;

import foundation.softwaredesign.orbi.model.game.character.CharacterDevelopment;
import foundation.softwaredesign.orbi.model.game.character.CharacterMessage;
import foundation.softwaredesign.orbi.persistence.repo.game.character.CharacterMessageRepository;
import foundation.softwaredesign.orbi.persistence.repo.game.character.CharacterMessageStatisticsRepository;

import javax.enterprise.context.RequestScoped;
import javax.inject.Inject;
import javax.ws.rs.WebApplicationException;
import java.util.ArrayList;
import java.util.List;

/**
 * @author Lucas Reeh <lr86gm@gmail.com>
 */
@RequestScoped
public class CharacterMessageService {

    private final static Long INBOX_MAX = new Long(10);

    @Inject
    CharacterMessageRepository repository;
    @Inject
    CharacterMessageStatisticsRepository statisticsRepository;
    @Inject
    CharacterService character;

    public Boolean isBusy(Long characterId) {
         return INBOX_MAX.compareTo(statisticsRepository.countByToCharacterId(characterId)) < 0;

    }

    public List<CharacterMessage> getMessages() {
        List<CharacterMessage> messages = repository.findByToCharacterId(character.loadCurrent().getId());
        for (CharacterMessage message : messages) {
            repository.remove(message);
        }
        if (messages == null) {
            return new ArrayList<>();
        }
        return messages;
    }

    public void createMessage(CharacterMessage message) {
        if (isBusy(message.getToCharacterId())) {
            throw new WebApplicationException("Character is busy or not available");
        }
        message.setFromCharacterId(character.loadCurrent().getId());
        character.incrementXp(CharacterDevelopment.XP_MESSAGE);
        repository.saveAndFlush(message);
    }


}
