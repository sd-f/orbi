package foundation.softwaredesign.orbi.persistence.repo;

import foundation.softwaredesign.orbi.model.Character;
import foundation.softwaredesign.orbi.model.GeoPosition;
import foundation.softwaredesign.orbi.model.Rotation;
import foundation.softwaredesign.orbi.persistence.entity.CharacterEntity;
import org.apache.deltaspike.data.api.mapping.SimpleQueryInOutMapperBase;

import static java.util.Objects.isNull;
import static java.util.Objects.nonNull;

/**
 * @author Lucas Reeh <lr86gm@gmail.com>
 */
public class CharacterMappper extends SimpleQueryInOutMapperBase<CharacterEntity, Character> {


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
        return dto;
    }

    @Override
    protected CharacterEntity toEntity(CharacterEntity characterEntity, Character character) {
        CharacterEntity newEntity = characterEntity;
        if (isNull(characterEntity)) {
            newEntity = new CharacterEntity();
        }
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
        return newEntity;
    }
}
