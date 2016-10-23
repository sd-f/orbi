package foundation.softwaredesign.orbi.persistence.repo.game;

import foundation.softwaredesign.orbi.model.game.server.MessageOfTheDay;
import foundation.softwaredesign.orbi.persistence.entity.MessageOfTheDayEntity;
import org.apache.deltaspike.data.api.mapping.SimpleQueryInOutMapperBase;

import static java.util.Objects.isNull;

/**
 * @author Lucas Reeh <lr86gm@gmail.com>
 */
public class MessageOfTheDayMappper extends SimpleQueryInOutMapperBase<MessageOfTheDayEntity, MessageOfTheDay> {

    @Override
    protected Object getPrimaryKey(MessageOfTheDay dto) {
        return dto.getId();
    }

    @Override
    protected MessageOfTheDay toDto(MessageOfTheDayEntity entity) {
        MessageOfTheDay dto = new MessageOfTheDay();
        dto.setId(entity.getId());
        dto.setMessage(entity.getMessage());
        dto.setCreated(entity.getCreated());
        return dto;
    }

    @Override
    protected MessageOfTheDayEntity toEntity(MessageOfTheDayEntity oldEntity, MessageOfTheDay dto) {
        MessageOfTheDayEntity newEntity = oldEntity;
        if (isNull(oldEntity)) {
            newEntity = new MessageOfTheDayEntity();
        }
        newEntity.setMessage(dto.getMessage());
        newEntity.setCreated(dto.getCreated());
        newEntity.setExpires(dto.getExpires());
        return newEntity;
    }
}
