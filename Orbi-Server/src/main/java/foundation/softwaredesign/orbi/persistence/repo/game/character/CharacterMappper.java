package foundation.softwaredesign.orbi.persistence.repo.game.character;

import foundation.softwaredesign.orbi.model.game.character.Character;
import foundation.softwaredesign.orbi.persistence.entity.CharacterEntity;
import foundation.softwaredesign.orbi.service.game.character.CharacterService;
import org.apache.deltaspike.data.api.mapping.SimpleQueryInOutMapperBase;

import javax.inject.Inject;

import static java.util.Objects.isNull;
import static java.util.Objects.nonNull;

/**
 * @author Lucas Reeh <lr86gm@gmail.com>
 */
public class CharacterMappper extends SimpleQueryInOutMapperBase<CharacterEntity, Character> {

    @Inject
    CharacterService characterService;

    @Override
    protected Object getPrimaryKey(Character character) {
        return character.getId();
    }

    @Override
    protected Character toDto(CharacterEntity entity) {
        Character dto = new Character();
        dto.setId(entity.getId());
        dto.setIdentityId(entity.getIdentityId());
        dto.getTransform().getGeoPosition().setLatitude(entity.getLatitude());
        dto.getTransform().getGeoPosition().setLongitude(entity.getLongitude());
        dto.getTransform().getGeoPosition().setAltitude(entity.getAltitude());
        dto.getTransform().getRotation().setY(entity.getRotationY());
        dto.getTransform().getRotation().setX(entity.getRotationX());
        dto.getTransform().getRotation().setZ(new Double(0));
        dto.setXp(entity.getExperiencePoints());
        dto.setName(entity.getName());
        dto.setLastSeen(entity.getLastSeen());
        dto.setGiftedOn(entity.getGiftedOn());
        characterService.calculateExperienceRank(dto);
        characterService.calculateLevel(dto);
        return dto;
    }

    @Override
    protected CharacterEntity toEntity(CharacterEntity characterEntity, Character character) {
        CharacterEntity newEntity = characterEntity;
        if (isNull(characterEntity)) {
            newEntity = new CharacterEntity();
        }
        newEntity.setId(character.getId());
        newEntity.setIdentityId(character.getIdentityId());
        if (nonNull(character.getTransform())) {
            if (nonNull(character.getTransform().getGeoPosition())) {
                newEntity.setLatitude(character.getTransform().getGeoPosition().getLatitude());
                newEntity.setLongitude(character.getTransform().getGeoPosition().getLongitude());
                newEntity.setAltitude(character.getTransform().getGeoPosition().getAltitude());
            }
            if (nonNull(character.getTransform().getRotation())) {
                newEntity.setRotationY(character.getTransform().getRotation().getY());
                newEntity.setRotationX(character.getTransform().getRotation().getX());
            }
        }
        newEntity.setLastSeen(character.getLastSeen());
        newEntity.setExperiencePoints(character.getXp());
        newEntity.setName(character.getName());
        newEntity.setGiftedOn(character.getGiftedOn());
        return newEntity;
    }
}
