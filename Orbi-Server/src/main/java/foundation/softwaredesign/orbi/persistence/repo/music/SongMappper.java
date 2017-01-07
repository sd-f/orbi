package foundation.softwaredesign.orbi.persistence.repo.music;

import foundation.softwaredesign.orbi.model.music.Song;
import foundation.softwaredesign.orbi.persistence.entity.music.SongEntity;
import foundation.softwaredesign.orbi.service.game.server.DateConverter;
import org.apache.deltaspike.data.api.mapping.SimpleQueryInOutMapperBase;

import javax.inject.Inject;

/**
 * @author Lucas Reeh <lr86gm@gmail.com>
 */
public class SongMappper extends SimpleQueryInOutMapperBase<SongEntity, Song> {

    @Inject
    DateConverter date;

    @Override
    protected Object getPrimaryKey(Song dto) {
        return dto.getId();
    }

    @Override
    protected Song toDto(SongEntity entity) {
        Song dto = new Song();
        dto.setId(entity.getId());
        dto.setTitle(entity.getTitle());
        dto.setUrl(entity.getUrl());
        return dto;
    }

    @Override
    protected SongEntity toEntity(SongEntity oldEntity, Song dto) {
        return null;
    }
}
