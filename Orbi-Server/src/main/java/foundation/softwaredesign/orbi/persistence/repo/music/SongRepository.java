package foundation.softwaredesign.orbi.persistence.repo.music;

import foundation.softwaredesign.orbi.model.music.Song;
import foundation.softwaredesign.orbi.persistence.entity.music.SongEntity;
import org.apache.deltaspike.data.api.EntityRepository;
import org.apache.deltaspike.data.api.Repository;
import org.apache.deltaspike.data.api.mapping.MappingConfig;

/**
 * @author Lucas Reeh <lr86gm@gmail.com>
 */
@Repository(forEntity = SongEntity.class)
@MappingConfig(SongMappper.class)
public interface SongRepository extends EntityRepository<Song, Long> {

}
