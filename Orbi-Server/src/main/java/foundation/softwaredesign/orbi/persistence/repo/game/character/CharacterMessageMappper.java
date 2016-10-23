package foundation.softwaredesign.orbi.persistence.repo.game.character;

import foundation.softwaredesign.orbi.model.game.character.CharacterMessage;
import foundation.softwaredesign.orbi.persistence.entity.CharacterMessageEntity;
import foundation.softwaredesign.orbi.service.game.character.CharacterService;
import org.apache.deltaspike.data.api.mapping.SimpleQueryInOutMapperBase;

import javax.inject.Inject;
import java.util.Date;

import static java.util.Objects.isNull;

/**
 * @author Lucas Reeh <lr86gm@gmail.com>
 */
public class CharacterMessageMappper extends SimpleQueryInOutMapperBase<CharacterMessageEntity, CharacterMessage> {

    @Inject
    CharacterService characterService;

    @Override
    protected Object getPrimaryKey(CharacterMessage character) {
        return character.getId();
    }

    @Override
    protected CharacterMessage toDto(CharacterMessageEntity entity) {
        CharacterMessage dto = new CharacterMessage();
        dto.setId(entity.getId());
        dto.setMessage(entity.getMessage());
        dto.setFromCharacterId(entity.getFromCharacterId());
        dto.setFromCharacter(characterService.loadById(entity.getFromCharacterId()).getName());
        dto.setToCharacterId(entity.getToCharacterId());
        dto.setToCharacter(characterService.loadById(entity.getToCharacterId()).getName());
        return dto;
    }

    @Override
    protected CharacterMessageEntity toEntity(CharacterMessageEntity oldEntity, CharacterMessage dto) {
        CharacterMessageEntity newEntity = oldEntity;
        if (isNull(oldEntity)) {
            newEntity = new CharacterMessageEntity();
        }
        newEntity.setCreatedOn(new Date());
        newEntity.setToCharacterId(dto.getToCharacterId());
        newEntity.setFromCharacterId(dto.getFromCharacterId());
        newEntity.setMessage(dto.getMessage());
        return newEntity;
    }
}
