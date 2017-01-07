package foundation.softwaredesign.orbi.service.music;

import foundation.softwaredesign.orbi.model.music.Song;
import foundation.softwaredesign.orbi.persistence.repo.music.SongRepository;

import javax.enterprise.context.RequestScoped;
import javax.inject.Inject;
import java.util.List;
import java.util.Random;

/**
 * @author Lucas Reeh <lr86gm@gmail.com>
 */
@RequestScoped
public class MusicService {

    @Inject
    SongRepository songRepository;

    private Random randomGenerator = new Random();

    public Song getNextRandomSong() {
        List<Song> songs = songRepository.findAll();
        int index = randomGenerator.nextInt(songs.size());
        return songs.get(index);
    }

}
